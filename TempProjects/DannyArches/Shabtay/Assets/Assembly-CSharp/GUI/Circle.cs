using UnityEngine;
using System.IO;
using Unity.VectorGraphics;
using static Unity.VectorGraphics.VectorUtils;

public static class Circle
{
    // Start is called before the first frame update
    public static GameObject CreateGameObject(string p_Name)
    {
        GameObject GOCircle = new GameObject(p_Name);
        GOCircle.transform.position = new Vector3(0, 0, 0);
       SpriteRenderer SRCircle = GOCircle.AddComponent<SpriteRenderer>();
        return GOCircle;
    }
    public static Sprite CreateSprite(float p_Radius,string p_stroke)
    {
        string svg =

            "<svg xmlns='http://www.w3.org/2000/svg' >"+
                "<circle  r='"+ p_Radius.ToString()+"'  stroke='"+ p_stroke+"' stroke-width='0.02' fill-opacity='0' id='circleId'/>" +
            "</svg>";

        // Import the SVG at runtime
        var sceneInfo = SVGParser.ImportSVG(new StringReader(svg));
        //var shape = sceneInfo.NodeIDs["circleId"].Shapes[0];
        //shape.Fill = new SolidFill() { Color = Color.red };
  
        // Tessellate
        TessellationOptions tessOptions = new VectorUtils.TessellationOptions()
        {
            StepDistance = 1.0f,
            MaxCordDeviation = 0.5f,
            MaxTanAngleDeviation = 0.1f,
            SamplingStepSize = 0.1f
        };

        var geoms = VectorUtils.TessellateScene(sceneInfo.Scene, tessOptions);

        // Build a sprite
        Sprite sprite = VectorUtils.BuildSprite(geoms, 0.1f, VectorUtils.Alignment.Center, Vector2.zero, 128, true);
        return sprite;
    }

}
