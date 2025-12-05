using UnityEngine;
using UnityEngine.UI;
namespace Project_Unorder.UIManage
{
#if UNITY_EDITOR
    using UnityEditor;
    public class ImageConvertMenu
    {
        [MenuItem("CONTEXT/Image/Convert to UIImage")]
        private static void Convert(MenuCommand command)
        {
            Image src = (Image)command.context;
            GameObject obj = src.gameObject;

            Undo.RecordObject(obj, "Convert Image To UIImage");

            var backup = new ImageData(src);
            Undo.DestroyObjectImmediate(src);
            var dst = Undo.AddComponent<UIImage>(obj);
            backup.ApplyTo(dst);

            Debug.Log($"Converted Image → UIImage ({obj.name})");
        }

        private struct ImageData
        {
            public Sprite sprite;
            public Color color;
            public Material material;
            public bool raycastTarget;
            public Image.Type type;
            public bool preserveAspect;
            public bool fillCenter;
            public Image.FillMethod fillMethod;
            public float fillAmount;
            public bool fillClockwise;
            public int fillOrigin;
            public float pixelPerUnit;

            public ImageData(Image src)
            {
                sprite = src.sprite;
                color = src.color;
                material = src.material;
                raycastTarget = src.raycastTarget;
                type = src.type;
                preserveAspect = src.preserveAspect;
                fillCenter = src.fillCenter;
                fillMethod = src.fillMethod;
                fillAmount = src.fillAmount;
                fillClockwise = src.fillClockwise;
                fillOrigin = src.fillOrigin;
                pixelPerUnit = src.pixelsPerUnitMultiplier;
            }

            public void ApplyTo(Image dst)
            {
                dst.sprite = sprite;
                dst.color = color;
                dst.material = material;
                dst.raycastTarget = raycastTarget;
                dst.type = type;
                dst.preserveAspect = preserveAspect;
                dst.fillCenter = fillCenter;
                dst.fillMethod = fillMethod;
                dst.fillAmount = fillAmount;
                dst.fillClockwise = fillClockwise;
                dst.fillOrigin = fillOrigin;
                dst.pixelsPerUnitMultiplier = pixelPerUnit;
            }
        }
    }
#endif


    public class UIImage : Image, IColorChangeable
    {
        public void ChangeColor(Color newColor)
        {
            color = newColor;
        }
    }
}