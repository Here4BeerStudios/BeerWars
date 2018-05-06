using Assets.Scripts.Ingame.Contents;
using System.Collections.Generic;
using UnityEngine;

public class ContentHandler : MonoBehaviour {
    public Dictionary<Content, CellContent> Contents { get; private set; }

	void Awake()
    {
        Contents = new Dictionary<Content, CellContent>();
        var sprites = Resources.LoadAll<Sprite>(@"Sprites/Cell/Content");
        var spritesDic = new Dictionary<string, Sprite>(sprites.Length);
        foreach (var sprite in sprites)
        {
            spritesDic.Add(sprite.name, sprite);
        }
        AddContent(Content.Normal, null);
        AddContent(Content.Cornfield, spritesDic["010-field"]);
        AddContent(Content.Water, spritesDic["004-sea"]);
        //AddContent(Content.Forest, spritesDic["Forest"]);
        //AddContent(Content.Hill, spritesDic["Hill"]);
        AddContent(Content.Brewery, spritesDic["001-brewery"]);
        //AddContent(Content.Village, spritesDic["Village"]);
    }

    private void AddContent(Content content, Sprite sprite)
    {
        Contents.Add(content, new CellContent(content, sprite));
    }
}
