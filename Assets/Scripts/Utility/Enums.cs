public abstract class Enums
{
    public enum AudioSfxType : short
    {
        CardFlip,
        CardMatch,
        CardMissMatch,
        GameOver,
        ButtonClick
    }

    public enum AudioAction : short
    {
        Play,
        PlayOneShot,
        Stop,
        Pause,
        Resume
    }
}
