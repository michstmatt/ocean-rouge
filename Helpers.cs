using System.Threading.Tasks;
using Godot;
public static class Helpers 
{
	public async static Task CreateAsyncTimer(this Node node, float time)
	{
		//await node.ToSignal(node.GetTree().CreateTimer(time), "timeout");
	}
}
