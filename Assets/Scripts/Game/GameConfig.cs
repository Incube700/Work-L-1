using UnityEngine;

[CreateAssetMenu(fileName = "NewGameConfig", menuName = "Game/Config")]
public class GameConfig : ScriptableObject 
{
    public GameMode Mode;
    public string AvailableChars; 
}