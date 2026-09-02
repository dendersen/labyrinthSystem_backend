using labyrinthSystem_backend;

public class MainClass
{
    public static void Main(string[] args)
    {
        int width = 10;
        int height = 10;
        Color black = new Color(0x808080);
        Draw text = new(width, height,true,true);
        text.UpdateBuff(1, 1, black, false);
        text.UpdateBuff(1, 2, black, false);
        text.UpdateBuff(1, 3, black, false);
        text.UpdateBuff(3, 1, black, false);
        text.UpdateBuff(3, 2, black, false);
        text.UpdateBuff(3, 3, black, false);
        text.UpdateBuff(2, 2, black, false);

        Draw screen = new(width,height,true,true);
        Random rnd = new Random();
        while(true) {
            for (int i = 0; i < height; i++)
            {
                for (int j = 0; j < width; j++)
                {
                    screen.UpdateBuff(
                        j, i, new Color(
                            (byte)(rnd.Next() & 0xff),
                            (byte)(rnd.Next() & 0xff),
                            (byte)(rnd.Next() & 0xff)
                        ),
                        false
                    );
                }
            }
            Console.WriteLine("\e[38;0;0m");
            Console.WriteLine("\nwidth: {0}, height: {1}", width, height);
            System.Threading.Thread.Sleep(50);
            Draw.DrawAll(new Draw[] { screen, text });
            if (width < height && width < 50)
            {
                width++;
                screen.UpdateSize(width, height);
                text.UpdateSize(width, height);
            }
            else if (height < 30)
            {
                height++;
                screen.UpdateSize(width, height);
                text.UpdateSize(width, height);
            }
            else if (height >= 30 && width < 50)
            {
                width++;
                screen.UpdateSize(width, height);
                text.UpdateSize(width, height);
            }
        }
    }
}