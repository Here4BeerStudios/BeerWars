using Assets.Scripts.Ingame.Contents;
using System.Collections.Generic;
using UnityEngine;

public class ContentHandler : MonoBehaviour
{
    private Dictionary<Content, CellContent> _contents;

	void Awake()
    {
        _contents = new Dictionary<Content, CellContent>();
        //Load content
        var sprites = Resources.LoadAll<Sprite>(@"Sprites/Cell/Content");
        var spritesDic = new Dictionary<string, Sprite>(sprites.Length);
        foreach (var sprite in sprites)
        {
            spritesDic.Add(sprite.name, sprite);
        }
        //Add content
        AddContent(Content.Normal, null);
        AddContent(Content.Cornfield, spritesDic["010-field"]);
        AddContent(Content.Water, spritesDic["004-sea"]);
        //AddContent(Content.Forest, spritesDic["Forest"]);
        //AddContent(Content.Hill, spritesDic["Hill"]);
        AddContent(Content.Brewery, spritesDic["001-brewery"]);
        AddContent(Content.Village, spritesDic["005-beer-3"]);   //todo fix village sprite
        AddContent(Content.Delivery, spritesDic["007-beer-2"]);   //todo fix village sprite
    }

    private void AddContent(Content content, Sprite sprite)
    {
        _contents.Add(content, new CellContent(content, sprite));
    }
    
    public CellContent this[Content content]
    {
        get { return _contents[content]; }
    }
}
