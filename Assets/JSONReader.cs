using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class JSONReader : MonoBehaviour
{
    private string jsonFilePath = "UITemplate.json";
    public RootObject rootObj = new RootObject();
    public Transform templateHolder;

    public void ShowUITemplate()
    {
        if (File.Exists(jsonFilePath))
        {
            string jsonData = File.ReadAllText(jsonFilePath);

            // Assuming your JSON data is stored in a string variable called jsonData
            rootObj = JsonConvert.DeserializeObject<RootObject>(jsonData);

            // Now you can access the parsed data using the rootObject instance
            foreach (var uiObject in rootObj.ui_objects)
            {
                GameObject gameObj = new GameObject();
                gameObj.name = uiObject.name;
                gameObj.transform.parent = templateHolder;

                switch (uiObject.type)
                {
                    case "image":
                        gameObj.AddComponent<Image>();
                        gameObj.GetComponent<Image>().color = Color.yellow;
                        break;

                    case "button":
                        gameObj.AddComponent<Button>();
                        gameObj.AddComponent<Image>();
                        gameObj.GetComponent<Image>().sprite = UnityEditor.AssetDatabase.GetBuiltinExtraResource<Sprite>("UI/Skin/UISprite.psd");
                        break;

                    case "text":
                        gameObj.AddComponent<Text>();
                        Text textComp = gameObj.GetComponent<Text>();
                        textComp.text = uiObject.text;
                        textComp.font = Resources.GetBuiltinResource(typeof(Font), "Arial.ttf") as Font;
                        textComp.color = Color.black;
                        textComp.fontSize = uiObject.textSize;
                        textComp.alignment = TextAnchor.MiddleCenter;
                        break;
                }
                gameObj.GetComponent<RectTransform>().localPosition = new Vector2(uiObject.position.x, uiObject.position.y);
                gameObj.GetComponent<RectTransform>().sizeDelta = new Vector2(uiObject.size.width, uiObject.size.height);


                if (uiObject.children != null && uiObject.children.Count > 0)
                {
                    foreach (var child in uiObject.children)
                    {
                        GameObject childObj = new GameObject();
                        childObj.name = child.name;
                        childObj.transform.parent = gameObj.transform;
                        switch (child.type)
                        {
                            case "image":
                                childObj.AddComponent<Image>();
                                break;

                            case "button":
                                childObj.AddComponent<Button>();
                                childObj.AddComponent<Image>();
                                childObj.GetComponent<Image>().sprite = UnityEditor.AssetDatabase.GetBuiltinExtraResource<Sprite>("UI/Skin/UISprite.psd");
                                break;

                            case "text":
                                childObj.AddComponent<Text>();
                                Text textComp = childObj.GetComponent<Text>();
                                textComp.text = child.text;
                                textComp.font = Resources.GetBuiltinResource(typeof(Font), "Arial.ttf") as Font;
                                textComp.color = Color.black;
                                textComp.fontSize = child.textSize;
                                textComp.alignment = TextAnchor.MiddleCenter;
                                break;
                        }
                        childObj.GetComponent<RectTransform>().localPosition = new Vector2(child.position.x, child.position.y);
                        childObj.GetComponent<RectTransform>().sizeDelta = new Vector2(child.size.width, child.size.height);
                    }
                }
            }
        }
        else
        {
            Debug.LogError("JSON file does not exist.");
        }
    }
}

public class UIObject
{
    public string name { get; set; }
    public string type { get; set; }
    public Position position { get; set; }
    public Size size { get; set; }
    public string text { get; set; }
    public string textColor { get; set; }
    public int textSize { get; set; }
    public List<UIObject> children { get; set; }
}

public class Position
{
    public int x { get; set; }
    public int y { get; set; }
}

public class Size
{
    public int width { get; set; }
    public int height { get; set; }
}

public class RootObject
{
    public List<UIObject> ui_objects { get; set; }
}

