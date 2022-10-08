using System.Text;

public interface IInflictable
{
    public StringBuilder inflictorInfo { get; }
    public void UpdateInflictorInfo();
}
