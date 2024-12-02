public class Player
{
    public string id;
    public int level;
    public int currentExp;
    public int kill;
    public int score;
    public int selectPlayer;
    public int idItemWeapon;
    public bool isCave;

    public Player() { }

    public Player (string id, int level, int currentExp, int kill, int score, int selectPlayer, int idItemWeapon, bool isCave)
    {
        this.id = id;
        this.level = level;
        this.currentExp = currentExp;
        this.kill = kill;
        this.score = score;
        this.selectPlayer = selectPlayer;
        this.idItemWeapon = idItemWeapon;
        this.isCave = isCave;
    }
}
