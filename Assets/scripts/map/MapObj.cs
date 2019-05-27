// 存储 map 数据对象
public class MapObj {
	public delegate void SpecialPlot(Player player);

	public string name = "";
	public int width = 0;
	public int height = 0;
	public int[] mapData = null;
	public SpecialPlot handle = null;

	public MapObj(int width, int height, int[] data, string name) {
		this.width = width;
		this.height = height;
		this.mapData = data;
		this.name = name;
	}
}
