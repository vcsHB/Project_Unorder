using UnityEngine;
using UnityEditor;
using UnityEngine.Localization.Tables;
using System;
using System.Collections.Generic;
using UnityEngine.Networking;
using System.Collections;
using System.Linq;
using UnityEditor.Localization;

namespace LocalizationTools
{
    /// <summary>
    /// 여러 Localization Table을 프로필로 관리하는 실시간 Importer
    /// </summary>
    public class MultiTableRealtimeImporter : EditorWindow
    {
        // 공통 Apps Script URL
        private string commonAppsScriptUrl = "";
        
        // 프로필 리스트
        private List<LocalizationProfile> profiles = new List<LocalizationProfile>();
        private Vector2 scrollPosition;
        private Vector2 profileScrollPosition;

        private bool isImporting = false;
        private string statusMessage = "";
        private int selectedProfileIndex = -1;

        // 새 프로필 추가 UI
        private bool showAddProfile = false;
        private string newProfileName = "";
        private StringTableCollection newProfileTable;

        [MenuItem("Tools/Localization/Multi-Table Importer")]
        public static void ShowWindow()
        {
            var window = GetWindow<MultiTableRealtimeImporter>("Multi-Table Importer");
            window.minSize = new Vector2(500, 400);
        }

        private void OnEnable()
        {
            LoadProfiles();
            commonAppsScriptUrl = EditorPrefs.GetString("LocalizationCommonAppsScriptUrl", "");
        }

        private void OnDisable()
        {
            SaveProfiles();
            EditorPrefs.SetString("LocalizationCommonAppsScriptUrl", commonAppsScriptUrl);
        }

        private void OnGUI()
        {
            scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition);

            EditorGUILayout.Space(10);
            EditorGUILayout.LabelField("Multi-Table Localization Importer", EditorStyles.boldLabel);
            EditorGUILayout.HelpBox(
                "여러 Localization Table을 프로필로 관리합니다.\n" +
                "하나의 Apps Script URL로 모든 시트를 관리할 수 있습니다.",
                MessageType.Info
            );
            EditorGUILayout.Space(10);

            // 공통 Apps Script URL 설정
            DrawCommonSettings();
            
            EditorGUILayout.Space(10);

            // Status Message
            if (!string.IsNullOrEmpty(statusMessage))
            {
                MessageType messageType = statusMessage.Contains("Error") || statusMessage.Contains("Failed")
                    ? MessageType.Error
                    : statusMessage.Contains("Success")
                        ? MessageType.Info
                        : MessageType.Warning;

                EditorGUILayout.HelpBox(statusMessage, messageType);
                EditorGUILayout.Space(5);
            }

            // 프로필 목록
            DrawProfilesList();

            EditorGUILayout.Space(10);

            // 일괄 작업 버튼
            DrawBatchActions();

            EditorGUILayout.Space(10);

            // 프로필 추가 섹션
            DrawAddProfileSection();

            EditorGUILayout.EndScrollView();
        }

        private void DrawCommonSettings()
        {
            EditorGUILayout.LabelField("Common Settings", EditorStyles.boldLabel);
            EditorGUILayout.BeginVertical(EditorStyles.helpBox);
            
            commonAppsScriptUrl = EditorGUILayout.TextField("Apps Script URL", commonAppsScriptUrl);
            
            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button("Test Connection", GUILayout.Width(120)))
            {
                TestCommonConnection();
            }
            EditorGUILayout.EndHorizontal();
            
            EditorGUILayout.EndVertical();
        }

        private void DrawProfilesList()
        {
            EditorGUILayout.LabelField($"Profiles ({profiles.Count})", EditorStyles.boldLabel);

            if (profiles.Count == 0)
            {
                EditorGUILayout.HelpBox("프로필이 없습니다. 새 프로필을 추가하세요.", MessageType.Warning);
                return;
            }

            profileScrollPosition = EditorGUILayout.BeginScrollView(profileScrollPosition, GUILayout.Height(300));

            for (int i = 0; i < profiles.Count; i++)
            {
                DrawProfile(i);
                EditorGUILayout.Space(5);
            }

            EditorGUILayout.EndScrollView();
        }

        private void DrawProfile(int index)
        {
            var profile = profiles[index];

            EditorGUILayout.BeginVertical(EditorStyles.helpBox);

            // 프로필 헤더
            EditorGUILayout.BeginHorizontal();

            profile.enabled = EditorGUILayout.Toggle(profile.enabled, GUILayout.Width(20));

            EditorGUILayout.LabelField(profile.name, EditorStyles.boldLabel);

            GUILayout.FlexibleSpace();

            // Import 버튼
            GUI.enabled = !isImporting && profile.enabled && profile.IsValid() && !string.IsNullOrEmpty(commonAppsScriptUrl);
            if (GUILayout.Button("Import", GUILayout.Width(70)))
            {
                ImportProfile(index);
            }
            GUI.enabled = true;

            // 삭제 버튼
            GUI.backgroundColor = new Color(1f, 0.5f, 0.5f);
            if (GUILayout.Button("×", GUILayout.Width(25)))
            {
                if (EditorUtility.DisplayDialog("Delete Profile",
                    $"'{profile.name}' 프로필을 삭제하시겠습니까?", "Delete", "Cancel"))
                {
                    profiles.RemoveAt(index);
                    SaveProfiles();
                    GUIUtility.ExitGUI();
                }
            }
            GUI.backgroundColor = Color.white;

            EditorGUILayout.EndHorizontal();

            // 프로필 상세 정보
            EditorGUI.indentLevel++;

            // Table Collection은 직접 할당하고 내부적으로 GUID로 저장
            var currentTable = profile.GetTableCollection();
            var newTable = (StringTableCollection)EditorGUILayout.ObjectField(
                "Table Collection",
                currentTable,
                typeof(StringTableCollection),
                false
            );
            
            if (newTable != currentTable)
            {
                profile.SetTableCollection(newTable);
            }

            profile.autoDetectSheetName = EditorGUILayout.Toggle("Auto Detect Sheet Name", profile.autoDetectSheetName);

            if (!profile.autoDetectSheetName)
            {
                EditorGUI.indentLevel++;
                profile.sheetName = EditorGUILayout.TextField("Sheet Name", profile.sheetName);
                EditorGUI.indentLevel--;
            }
            else if (currentTable != null)
            {
                EditorGUI.indentLevel++;
                EditorGUILayout.LabelField("Sheet Name (Auto)", currentTable.TableCollectionName, EditorStyles.miniLabel);
                EditorGUI.indentLevel--;
            }

            profile.overwriteExisting = EditorGUILayout.Toggle("Overwrite Existing", profile.overwriteExisting);
            profile.createMissingKeys = EditorGUILayout.Toggle("Create Missing Keys", profile.createMissingKeys);

            // 마지막 Import 정보
            if (profile.lastImportInfo != null)
            {
                EditorGUILayout.LabelField("Last Import:", EditorStyles.miniLabel);
                EditorGUI.indentLevel++;
                EditorGUILayout.LabelField($"Time: {profile.lastImportInfo.timestamp}", EditorStyles.miniLabel);
                EditorGUILayout.LabelField($"Entries: {profile.lastImportInfo.count}", EditorStyles.miniLabel);
                EditorGUILayout.LabelField($"Languages: {profile.lastImportInfo.languages}", EditorStyles.miniLabel);
                EditorGUI.indentLevel--;
            }

            EditorGUI.indentLevel--;

            EditorGUILayout.EndVertical();
        }

        private void DrawBatchActions()
        {
            EditorGUILayout.LabelField("Batch Actions", EditorStyles.boldLabel);

            EditorGUILayout.BeginHorizontal();

            GUI.enabled = !isImporting && profiles.Any(p => p.enabled && p.IsValid()) && !string.IsNullOrEmpty(commonAppsScriptUrl);
            if (GUILayout.Button("Import All Enabled", GUILayout.Height(35)))
            {
                ImportAllEnabled();
            }
            GUI.enabled = true;

            if (GUILayout.Button("Enable All", GUILayout.Height(35)))
            {
                foreach (var profile in profiles)
                    profile.enabled = true;
            }

            if (GUILayout.Button("Disable All", GUILayout.Height(35)))
            {
                foreach (var profile in profiles)
                    profile.enabled = false;
            }

            EditorGUILayout.EndHorizontal();
        }

        private void DrawAddProfileSection()
        {
            EditorGUILayout.LabelField("Add New Profile", EditorStyles.boldLabel);

            showAddProfile = EditorGUILayout.Foldout(showAddProfile, "New Profile");

            if (showAddProfile)
            {
                EditorGUI.indentLevel++;

                newProfileName = EditorGUILayout.TextField("Profile Name", newProfileName);
                newProfileTable = (StringTableCollection)EditorGUILayout.ObjectField(
                    "Table Collection",
                    newProfileTable,
                    typeof(StringTableCollection),
                    false
                );

                EditorGUILayout.Space(5);

                GUI.enabled = !string.IsNullOrEmpty(newProfileName) && newProfileTable != null;

                if (GUILayout.Button("Add Profile", GUILayout.Height(30)))
                {
                    AddNewProfile();
                }

                GUI.enabled = true;

                EditorGUI.indentLevel--;
            }
        }

        private void AddNewProfile()
        {
            var profile = new LocalizationProfile
            {
                name = newProfileName,
                enabled = true,
                overwriteExisting = true,
                createMissingKeys = true,
                autoDetectSheetName = true
            };
            
            profile.SetTableCollection(newProfileTable);

            profiles.Add(profile);
            SaveProfiles();

            // 초기화
            newProfileName = "";
            newProfileTable = null;
            showAddProfile = false;

            statusMessage = $"Profile '{profile.name}' added successfully!";
        }

        private void TestCommonConnection()
        {
            if (string.IsNullOrEmpty(commonAppsScriptUrl))
            {
                statusMessage = "Error: Please enter Apps Script URL";
                return;
            }
            
            EditorCoroutineRunner.StartCoroutine(TestConnectionCoroutine());
        }

        private IEnumerator TestConnectionCoroutine()
        {
            isImporting = true;
            statusMessage = "Testing connection...";
            Repaint();

            string url = commonAppsScriptUrl + "?action=ping";

            using (UnityWebRequest request = UnityWebRequest.Get(url))
            {
                request.redirectLimit = 10;
                request.timeout = 15;

                var operation = request.SendWebRequest();

                while (!operation.isDone)
                {
                    yield return null;
                }

                Debug.Log($"[Request Finished] URL: {url}");

                if (request.result != UnityWebRequest.Result.Success)
                {
                    string details = $"Code: {request.responseCode} | Error: {request.error}";

                    if (request.responseCode == 0 && string.IsNullOrEmpty(request.error))
                    {
                        details += " (Possible Network/SSL Issue or Request Aborted)";
                    }

                    statusMessage = $"Connection Failed: {details}";
                    Debug.LogWarning($"[Connection Test Fail] {details}");
                }
                else
                {
                    string responseText = request.downloadHandler.text;
                    statusMessage = "Connection Successful!";
                    Debug.Log($"[Connection Test Success] Response: {responseText}");
                }
            }

            isImporting = false;
            Repaint();
        }

        private void ImportProfile(int index)
        {
            selectedProfileIndex = index;
            EditorCoroutineRunner.StartCoroutine(ImportProfileCoroutine(profiles[index]));
        }

        private void ImportAllEnabled()
        {
            EditorCoroutineRunner.StartCoroutine(ImportAllEnabledCoroutine());
        }

        private IEnumerator ImportAllEnabledCoroutine()
        {
            isImporting = true;
            var enabledProfiles = profiles.Where(p => p.enabled && p.IsValid()).ToList();

            statusMessage = $"Importing {enabledProfiles.Count} profiles...";
            Repaint();

            int successCount = 0;
            int failCount = 0;

            foreach (var profile in enabledProfiles)
            {
                statusMessage = $"Importing '{profile.name}'...";
                Repaint();

                yield return ImportProfileCoroutine(profile, false);

                if (statusMessage.Contains("Success"))
                    successCount++;
                else
                    failCount++;

                yield return new EditorWaitForSeconds(0.5f);
            }

            statusMessage = $"Batch Import Complete - Success: {successCount}, Failed: {failCount}";
            isImporting = false;
            Repaint();
        }

        private IEnumerator ImportProfileCoroutine(LocalizationProfile profile, bool updateStatus = true)
        {
            if (updateStatus)
            {
                isImporting = true;
                statusMessage = $"Fetching data for '{profile.name}'...";
                Repaint();
            }

            var tableCollection = profile.GetTableCollection();
            if (tableCollection == null)
            {
                statusMessage = $"Error ({profile.name}): Table Collection not found!";
                Debug.LogError($"[{profile.name}] Table Collection reference is broken. GUID: {profile.tableCollectionGuid}");
                if (updateStatus) isImporting = false;
                Repaint();
                yield break;
            }

            string sheetName = profile.GetSheetName();
            string url = commonAppsScriptUrl + "?action=getSheetData&sheet=" + UnityWebRequest.EscapeURL(sheetName) + "&t=" + DateTime.Now.Ticks;

            Debug.Log($"[{profile.name}] Requesting: {url}");

            UnityWebRequest request = UnityWebRequest.Get(url);
            request.timeout = 30;
            request.redirectLimit = 10;

            var operation = request.SendWebRequest();

            while (!operation.isDone)
            {
                yield return null;
            }

            Debug.Log($"[{profile.name}] Request completed - Result: {request.result}, Code: {request.responseCode}");

            if (request.result != UnityWebRequest.Result.Success)
            {
                string errorDetails = GetDetailedErrorMessage(request, profile);
                statusMessage = $"Error ({profile.name}): {errorDetails}";
                Debug.LogError($"[{profile.name}] Request failed: {errorDetails}\nURL: {url}");
                request.Dispose();
                if (updateStatus) isImporting = false;
                Repaint();
                yield break;
            }

            string jsonResponse = request.downloadHandler.text;
            long responseCode = request.responseCode;
            request.Dispose();

            Debug.Log($"[{profile.name}] Response code: {responseCode}");
            Debug.Log($"[{profile.name}] Response: {jsonResponse.Substring(0, Math.Min(500, jsonResponse.Length))}...");

            List<LocalizationEntry> parsedData = null;

            try
            {
                parsedData = ParseJsonResponse(jsonResponse);

                var jsonObj = MiniJson.JsonDecode(jsonResponse) as Dictionary<string, object>;
                if (jsonObj != null)
                {
                    if (jsonObj.ContainsKey("error"))
                    {
                        string errorMsg = jsonObj["error"].ToString();
                        statusMessage = $"API Error ({profile.name}): {errorMsg}";

                        if (jsonObj.ContainsKey("availableSheets"))
                        {
                            var sheets = jsonObj["availableSheets"] as List<object>;
                            if (sheets != null && sheets.Count > 0)
                            {
                                string sheetList = string.Join(", ", sheets.ConvertAll(x => x.ToString()));
                                statusMessage += $"\nAvailable sheets: {sheetList}";
                                Debug.LogWarning($"[{profile.name}] Sheet '{sheetName}' not found. Available: {sheetList}");
                            }
                        }

                        if (updateStatus) isImporting = false;
                        Repaint();
                        yield break;
                    }

                    profile.lastImportInfo = new ApiResponseInfo
                    {
                        languages = jsonObj.ContainsKey("languages")
                            ? string.Join(", ", (jsonObj["languages"] as List<object>).ConvertAll(x => x.ToString()))
                            : "",
                        count = jsonObj.ContainsKey("count") ? Convert.ToInt32(jsonObj["count"]) : 0,
                        timestamp = jsonObj.ContainsKey("timestamp") ? jsonObj["timestamp"].ToString() : DateTime.Now.ToString()
                    };
                }
            }
            catch (Exception ex)
            {
                statusMessage = $"Parse Error ({profile.name}): {ex.Message}\nCheck Console for details.";
                Debug.LogError($"[{profile.name}] Parse failed: {ex}\nResponse: {jsonResponse}");
                if (updateStatus) isImporting = false;
                Repaint();
                yield break;
            }

            if (parsedData == null || parsedData.Count == 0)
            {
                statusMessage = $"Warning ({profile.name}): No data in sheet '{sheetName}'";
                Debug.LogWarning($"[{profile.name}] Sheet '{sheetName}' is empty or has no valid data");
                if (updateStatus) isImporting = false;
                Repaint();
                yield break;
            }

            try
            {
                ImportToLocalizationTable(profile, parsedData, tableCollection);

                statusMessage = $"Success ({profile.name}): Imported {parsedData.Count} entries from sheet '{sheetName}'.";
                Debug.Log($"[{profile.name}] Successfully imported {parsedData.Count} entries");

                EditorUtility.SetDirty(tableCollection);
                AssetDatabase.SaveAssets();
                SaveProfiles();
            }
            catch (Exception ex)
            {
                statusMessage = $"Import Error ({profile.name}): {ex.Message}";
                Debug.LogError($"[{profile.name}] Import failed: {ex}");
            }

            if (updateStatus)
            {
                isImporting = false;
                Repaint();
            }
        }

        private string GetDetailedErrorMessage(UnityWebRequest request, LocalizationProfile profile)
        {
            string baseError = request.error;

            if (request.result == UnityWebRequest.Result.ConnectionError)
            {
                return "Connection Failed - Possible causes:\n" +
                       "1. Apps Script URL is incorrect\n" +
                       "2. No internet connection\n" +
                       "3. Firewall blocking the request\n" +
                       "Check Console for full URL";
            }

            if (request.result == UnityWebRequest.Result.ProtocolError)
            {
                long code = request.responseCode;
                string response = request.downloadHandler?.text ?? "";

                if (code == 404)
                {
                    return "404 Not Found - Apps Script deployment not found or deleted";
                }
                else if (code == 403)
                {
                    return "403 Forbidden - Check Apps Script deployment settings:\n" +
                           "- Must be deployed as 'Web app'\n" +
                           "- Access: 'Anyone' or 'Anyone with the link'";
                }
                else if (code == 302 || code == 301)
                {
                    return "Redirect detected - Make sure you're using the correct deployment URL\n" +
                           "(Use the 'Web app' URL, not the script.google.com/... URL)";
                }
                else
                {
                    return $"HTTP {code} - {baseError}\nResponse: {response.Substring(0, Math.Min(200, response.Length))}";
                }
            }

            if (request.result == UnityWebRequest.Result.DataProcessingError)
            {
                return "Data Processing Error - Response might not be valid JSON";
            }

            return baseError;
        }

        private List<LocalizationEntry> ParseJsonResponse(string json)
        {
            var entries = new List<LocalizationEntry>();

            try
            {
                var jsonObj = MiniJson.JsonDecode(json) as Dictionary<string, object>;

                if (jsonObj == null || !jsonObj.ContainsKey("entries"))
                    return entries;

                var entriesArray = jsonObj["entries"] as List<object>;
                if (entriesArray == null)
                    return entries;

                foreach (var entryObj in entriesArray)
                {
                    var entryDict = entryObj as Dictionary<string, object>;
                    if (entryDict == null) continue;

                    var entry = new LocalizationEntry
                    {
                        key = entryDict["key"].ToString(),
                        translations = new Dictionary<string, string>()
                    };

                    if (entryDict.ContainsKey("translations"))
                    {
                        var translationsDict = entryDict["translations"] as Dictionary<string, object>;
                        if (translationsDict != null)
                        {
                            foreach (var kvp in translationsDict)
                            {
                                entry.translations[kvp.Key] = kvp.Value?.ToString() ?? "";
                            }
                        }
                    }

                    entries.Add(entry);
                }
            }
            catch (Exception ex)
            {
                Debug.LogError($"JSON parsing error: {ex.Message}");
            }

            return entries;
        }

        private void ImportToLocalizationTable(LocalizationProfile profile, List<LocalizationEntry> entries, StringTableCollection tableCollection)
        {
            if (tableCollection == null || entries == null)
                return;

            int importedCount = 0;
            int updatedCount = 0;
            int skippedCount = 0;

            foreach (var entry in entries)
            {
                bool entryExists = tableCollection.SharedData.Contains(entry.key);

                if (entryExists && !profile.overwriteExisting)
                {
                    skippedCount++;
                    continue;
                }

                if (!entryExists && !profile.createMissingKeys)
                {
                    skippedCount++;
                    continue;
                }

                if (!entryExists)
                {
                    tableCollection.SharedData.AddKey(entry.key);
                    importedCount++;
                }
                else
                {
                    updatedCount++;
                }

                foreach (var table in tableCollection.StringTables)
                {
                    if (table == null) continue;

                    string localeCode = table.LocaleIdentifier.Code;

                    if (entry.translations.ContainsKey(localeCode))
                    {
                        string translation = entry.translations[localeCode];
                        var tableEntry = table.GetEntry(entry.key);

                        if (tableEntry != null)
                        {
                            tableEntry.Value = translation;
                        }
                        else
                        {
                            table.AddEntry(entry.key, translation);
                        }

                        EditorUtility.SetDirty(table);
                    }
                }
            }

            Debug.Log($"[{profile.name}] Import - New: {importedCount}, Updated: {updatedCount}, Skipped: {skippedCount}");
        }

        private void SaveProfiles()
        {
            var data = new ProfileData { profiles = profiles };
            string json = EditorJsonUtility.ToJson(data, true);
            EditorPrefs.SetString("LocalizationProfiles", json);
        }

        private void LoadProfiles()
        {
            string json = EditorPrefs.GetString("LocalizationProfiles", "");
            if (!string.IsNullOrEmpty(json))
            {
                try
                {
                    var data = JsonUtility.FromJson<ProfileData>(json);
                    if (data != null && data.profiles != null)
                    {
                        profiles = data.profiles;
                    }
                }
                catch
                {
                    profiles = new List<LocalizationProfile>();
                }
            }
        }

        [Serializable]
        private class ProfileData
        {
            public List<LocalizationProfile> profiles;
        }

        private class LocalizationEntry
        {
            public string key;
            public Dictionary<string, string> translations;
        }
    }

    [Serializable]
    public class LocalizationProfile
    {
        public string name;
        public string sheetName;
        public string tableCollectionGuid;  // GUID로 저장하여 레퍼런스 유지
        public bool enabled = true;
        public bool overwriteExisting = true;
        public bool createMissingKeys = true;
        public bool autoDetectSheetName = true;

        [NonSerialized]
        public ApiResponseInfo lastImportInfo;

        [NonSerialized]
        private StringTableCollection cachedTableCollection;

        public bool IsValid()
        {
            return GetTableCollection() != null;
        }

        public StringTableCollection GetTableCollection()
        {
            if (cachedTableCollection != null)
                return cachedTableCollection;

            if (string.IsNullOrEmpty(tableCollectionGuid))
                return null;

            string path = AssetDatabase.GUIDToAssetPath(tableCollectionGuid);
            if (string.IsNullOrEmpty(path))
                return null;

            cachedTableCollection = AssetDatabase.LoadAssetAtPath<StringTableCollection>(path);
            return cachedTableCollection;
        }

        public void SetTableCollection(StringTableCollection table)
        {
            cachedTableCollection = table;
            if (table != null)
            {
                tableCollectionGuid = AssetDatabase.AssetPathToGUID(AssetDatabase.GetAssetPath(table));
            }
            else
            {
                tableCollectionGuid = "";
            }
        }

        public string GetSheetName()
        {
            if (!string.IsNullOrEmpty(sheetName))
                return sheetName;

            if (autoDetectSheetName)
            {
                var tableCollection = GetTableCollection();
                if (tableCollection != null)
                    return tableCollection.TableCollectionName;
            }

            return "";
        }
    }

    public class ApiResponseInfo
    {
        public string languages;
        public int count;
        public string timestamp;
    }

    public class EditorWaitForSeconds
    {
        public float duration;

        public EditorWaitForSeconds(float seconds)
        {
            duration = seconds;
        }
    }

    public static class EditorCoroutineRunner
    {
        private class CoroutineState
        {
            public IEnumerator enumerator;
            public float waitTime;
        }

        private static List<CoroutineState> activeCoroutines = new List<CoroutineState>();

        public static void StartCoroutine(IEnumerator coroutine)
        {
            var state = new CoroutineState { enumerator = coroutine };
            activeCoroutines.Add(state);
            EditorApplication.update += UpdateCoroutines;
        }

        private static void UpdateCoroutines()
        {
            for (int i = activeCoroutines.Count - 1; i >= 0; i--)
            {
                var state = activeCoroutines[i];

                if (state.waitTime > 0)
                {
                    state.waitTime -= Time.deltaTime;
                    if (state.waitTime > 0)
                        continue;
                }

                bool shouldContinue = false;
                try
                {
                    shouldContinue = state.enumerator.MoveNext();

                    if (shouldContinue && state.enumerator.Current is EditorWaitForSeconds wait)
                    {
                        state.waitTime = wait.duration;
                    }
                }
                catch (Exception ex)
                {
                    Debug.LogError($"Coroutine error: {ex}");
                    shouldContinue = false;
                }

                if (!shouldContinue)
                {
                    activeCoroutines.RemoveAt(i);
                }
            }

            if (activeCoroutines.Count == 0)
            {
                EditorApplication.update -= UpdateCoroutines;
            }
        }
    }
}