using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Media;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace amazyngmove
{
    internal class Program
    {
        static void Main(string[] args)
        {
            int themenu = 0;
            Console.CursorVisible = false;//To make the cursor invisible
            ConsoleKeyInfo cki;
            SoundPlayer opensound = new SoundPlayer("pacmanopen.wav");
            SoundPlayer deadsound = new SoundPlayer("pacmandead.wav");
            SoundPlayer eatsound = new SoundPlayer("pacmaneat.wav");
            SoundPlayer minesound = new SoundPlayer("mineexplosion.wav");
            string sa = @"  ███████████████████████████████████████████████████████████████████████████████████████████
            █     █  █     █        █     █  █  █  █                    █     █              █        █
            █  ████  █  ███████  █  █  ████  █  █  ████  ███████  █  █  █  ███████  ████  ███████  █  █
            █  █              █  █  █     █     █  █  █  █  █     █  █        █     █              █  █
            █  ████  ████  ███████  █  ███████  █  █  █  █  ███████  █  █  █  ██████████  █  ████  █  █
            █     █     █     █  █        █  █        █        █     █  █  █  █     █  █  █  █  █  █  █
            █  █  ███████  ████  ███████  █  █  ████  █  ███████████████████  █  █  █  █  ████  ████  █
            █  █     █        █     █           █        █  █  █        █        █     █        █     █
            █  ████  █  ███████  ███████ ███████████████████████████████████████ ███████  ████  █  ████
            █  █           █             ██                                   ██    █         █ █     █
            ████  ██████████  ███████  █ ██                                   ████  █  █  █  ███████  █
            █           █     █  █  █  █ ██                                   ██    █  █  █  █     █  █
            █  █  ██████████  █  █  ████ ██                                   ████  ████  ████  ███████
            █  █     █  █     █     █    ██                                   ██                   █  █
            █  ████  █  ████  ████  ███████                                   ████████████    ██████  █
            █  █  █        █  █          ██                                   ███     █  █        █   █
            ████  █  █  ██████████  ███████                                   ████  █  █  ████  ████  █
            █        █        █        █ ██                                   ██       █     █     █  █
            █  █████████████  ████  ████ ███████████████████████████████████████ █  █  █  ██████████  █
            █     █  █     █     █  █  █                 █     █     █  █     █  █  █     █           █
            █  ████  ████  █  █  █  █  ███████  █  ███████  █  █  ███████  ████████████████  ██████████
            █     █        █  █  █  █  █     █  █        █  █  █        █     █        █     █        █
            ████  ███████  ████  █  █  █  █  █  █  █  ███████  ███████  █  ████  ████  █  ████  █  █  █
            █     █  █                 █  █     █  █  █  █  █     █  █     █     █     █     █  █  █  █
            █  █  █  █  ████  ███████  ███████  █  █  █  █  █  ████  ████  ████  ███████  █  ████  █  █
            █  █     █  █        █        █     █  █     █  █  █     █        █     █  █  █     █  █  █
            █  █  █     █           █        █        █  █              █           █     █     █  █  █
            ███████████████████████████████████████████████████████████████████████████████████████████ ";
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.SetCursorPosition(10, 0);
            Console.WriteLine(sa);
            while (themenu == 0)
            {
                int inspection = 0;
                string[] lines = System.IO.File.ReadAllLines("highestscore.txt");//For ht ehighest score
                string[] saveline = new string[lines.Length + 1];
                for (int i = 0; i < lines.Length; i++)//It ms-akes them 0 later and we will need the saved version
                    saveline[i] = lines[i];
                saveline[saveline.Length - 1] = "a";
                int scoreamount = lines.Length;
                int loopy = 5;//Loopy is for the cases there are less than 5 scores in file.
                if (scoreamount < 5)
                    loopy = scoreamount;
                int[] maxscore = { 0, 0, 0, 0, 0 };
                for (int i = 0; i < loopy; i++)
                {
                    int maximum = 0;
                    for (int j = 0; j < scoreamount; j++)
                        maximum = (int)Math.Max(maximum, Convert.ToInt64(lines[j]));//Finds the 5 highest scores and put them into an array.
                    maxscore[i] = maximum;
                    for (int k = 0; k < scoreamount; k++)
                    {
                        if (maxscore[i] == Convert.ToInt64(lines[k]))
                            lines[k] = "0";//Makes it 0 so all 5 maximum wont be the same.
                    }
                }
                Console.ForegroundColor = ConsoleColor.Green;//Printing the main menu
                Console.SetCursorPosition(47, 10);
                Console.WriteLine("Wellcome to Walls and Mines");
                Console.SetCursorPosition(51, 12);
                Console.WriteLine("Press Enter to play");
                Console.SetCursorPosition(47, 14);
                Console.WriteLine("Press L to see Highest Score");
                Console.SetCursorPosition(51, 16);
                Console.WriteLine("Press escape to Exit");
                if (Console.KeyAvailable)
                {
                    cki = Console.ReadKey(true);
                    if (cki.Key == ConsoleKey.Enter)//Starting the game.
                    {
                        int menureturner = 0;
                        while (menureturner == 0)//For the case player wants to play again
                        {
                            opensound.Play();
                            Console.Clear();
                            char[,] area = new char[56, 26];//Main map array
                            Console.ForegroundColor = ConsoleColor.DarkBlue;
                            for (int i = 0; i < area.GetLength(0); i++)
                            {
                                for (int j = 0; j < area.GetLength(1); j++)
                                {
                                    area[i, j] = ' ';
                                }
                            }
                            for (int n = 3; n < area.GetLength(0); n++)//Making outwalls of map
                                area[n, 3] = '#';
                            for (int i = 0; i < 21; i++)
                            {
                                area[3, 4 + i] = '#';
                                area[55, 4 + i] = '#';
                            }
                            for (int n = 3; n < area.GetLength(0); n++)
                                area[n, 25] = '#';
                            int wallcursory = 0;
                            Random random = new Random();
                            for (int rowdecide = 1; rowdecide < 9; rowdecide++)//Going through corner points of all cores
                            {
                                for (int columndecide = 1; columndecide < 21; columndecide++)
                                {
                                    int fiftypercent = random.Next(1, 3);//50 percent chance for a wall
                                    if (fiftypercent == 1)
                                    {
                                        if (rowdecide % 2 == 0)
                                            wallcursory = rowdecide / 2 * 5 + 3;
                                        if (rowdecide % 2 == 1)
                                        {
                                            if (rowdecide == 1)
                                                wallcursory = rowdecide * 5;
                                            else wallcursory = rowdecide * 5 - 5 * (rowdecide - 2);
                                        }
                                        int wallcursorx = 0;
                                        if (columndecide % 2 == 0)
                                            wallcursorx = columndecide / 2 * 5 + 3;
                                        if (columndecide % 2 == 1)
                                        {
                                            if (columndecide == 1)
                                                wallcursorx = columndecide * 5;
                                            else wallcursorx = columndecide * 5 - 5 * (columndecide - 2);
                                        }
                                        if ((wallcursorx % 5 == 0) && (wallcursory % 5 == 0))
                                        {
                                            area[wallcursorx, wallcursory] = '#';
                                            area[wallcursorx + 1, wallcursory] = '#';
                                            area[wallcursorx + 2, wallcursory] = '#';
                                            area[wallcursorx + 3, wallcursory] = '#';
                                        }
                                        else if ((wallcursorx % 5 != 0) && (wallcursory % 5 == 0))
                                        {
                                            area[wallcursorx, wallcursory] = '#';
                                            area[wallcursorx, wallcursory + 1] = '#';
                                            area[wallcursorx, wallcursory + 2] = '#';
                                            area[wallcursorx, wallcursory + 3] = '#';
                                        }
                                        else if ((wallcursorx % 5 == 0) && (wallcursory % 5 != 0))
                                        {
                                            area[wallcursorx, wallcursory] = '#';
                                            area[wallcursorx, wallcursory - 1] = '#';
                                            area[wallcursorx, wallcursory - 2] = '#';
                                            area[wallcursorx, wallcursory - 3] = '#';
                                        }
                                        else if ((wallcursorx % 5 != 0) && (wallcursory % 5 != 0))
                                        {
                                            area[wallcursorx, wallcursory] = '#';
                                            area[wallcursorx - 1, wallcursory] = '#';
                                            area[wallcursorx - 2, wallcursory] = '#';
                                            area[wallcursorx - 3, wallcursory] = '#';
                                        }
                                    }
                                }
                            }
                            for (int i = 6; i < area.GetLength(0); i += 5)//Going through all again to see if there are more or less walls than needed
                            {
                                for (int s = 6; s < area.GetLength(1); s += 5)
                                {
                                    if ((area[i - 1, s] != '#') && (area[i, s - 1] != '#') && (area[i + 2, s] != '#') && (area[i, s + 2] != '#'))
                                    {
                                        int allemptywallmaker = random.Next(1, 5);
                                        if (allemptywallmaker == 1)
                                        {
                                            area[i - 1, s - 1] = '#';
                                            area[i - 1, s] = '#';
                                            area[i - 1, s + 1] = '#';
                                            area[i - 1, s + 2] = '#';
                                        }
                                        if (allemptywallmaker == 2)
                                        {
                                            area[i - 1, s - 1] = '#';
                                            area[i, s - 1] = '#';
                                            area[i + 1, s - 1] = '#';
                                            area[i + 2, s - 1] = '#';
                                        }
                                        if (allemptywallmaker == 3)
                                        {
                                            area[i + 2, s - 1] = '#';
                                            area[i + 2, s] = '#';
                                            area[i + 2, s + 1] = '#';
                                            area[i + 2, s + 2] = '#';
                                        }
                                        if (allemptywallmaker == 4)
                                        {
                                            area[i - 1, s + 2] = '#';
                                            area[i, s + 2] = '#';
                                            area[i + 1, s + 2] = '#';
                                            area[i + 2, s + 2] = '#';
                                        }
                                    }
                                    else if ((area[i - 1, s] == '#') && (area[i, s - 1] == '#') && (area[i + 2, s] == '#') && (area[i, s + 2] == '#'))
                                    {
                                        int allemptywallmaker = random.Next(1, 5);
                                        if (allemptywallmaker == 1)
                                        {
                                            area[i - 1, s] = ' ';
                                            area[i - 1, s + 1] = ' ';
                                        }
                                        if (allemptywallmaker == 2)
                                        {
                                            area[i, s - 1] = ' ';
                                            area[i + 1, s - 1] = ' ';
                                        }
                                        if (allemptywallmaker == 3)
                                        {
                                            area[i + 2, s] = ' ';
                                            area[i + 2, s + 1] = ' ';
                                        }
                                        if (allemptywallmaker == 4)
                                        {
                                            area[i, s + 2] = ' ';
                                            area[i + 1, s + 2] = ' ';
                                        }
                                    }
                                }
                            }
                            int placer = 0, px = 0, py = 0;
                            for (int i = 0; i < area.GetLength(0); i++)
                            {
                                for (int f = 0; f < area.GetLength(1); f++)
                                {
                                    Console.SetCursorPosition(i, f);//Printing the map with colors
                                    if (area[i, f] != ' ')
                                        Console.BackgroundColor = ConsoleColor.DarkBlue;
                                    else
                                        Console.BackgroundColor = ConsoleColor.Black;
                                    Console.WriteLine(area[i, f]);
                                }
                            }
                            Console.BackgroundColor = ConsoleColor.Black;
                            Console.ForegroundColor = ConsoleColor.DarkCyan;
                            Console.SetCursorPosition(70, 5);//Printing the table
                            Console.WriteLine("Time: ");
                            Console.SetCursorPosition(70, 7);
                            Console.WriteLine("Energy: ");
                            Console.SetCursorPosition(70, 9);
                            Console.WriteLine("Score: ");
                            Console.SetCursorPosition(70, 11);
                            Console.WriteLine("Mine amount: ");
                            Console.ForegroundColor = ConsoleColor.DarkYellow;
                            while (placer != 1)
                            {
                                px = random.Next(4, 55);//placing player
                                py = random.Next(4, 25);
                                if (area[px, py] != '#')
                                {
                                    area[px, py] = '©';
                                    placer = 1;
                                    Console.SetCursorPosition(px, py);
                                    Console.WriteLine("©");
                                }
                                else
                                    placer = 0;
                            }
                            int xx = 0, xy = 0;
                            Console.ForegroundColor = ConsoleColor.DarkRed;
                            for (int i = 0; i < 2; i++)
                            {
                                placer = 0;
                                while (placer != 1)//Placing x's
                                {
                                    xx = random.Next(4, 55);
                                    xy = random.Next(4, 25);
                                    if (area[xx, xy] != '#' && area[xx, xy] != 'X' && area[xx, xy] != 'Y')
                                    {
                                        area[xx, xy] = 'X';
                                        placer = 1;
                                        Console.SetCursorPosition(xx, xy);
                                        Console.WriteLine("X");
                                    }
                                    else
                                        placer = 0;
                                }
                                placer = 0;
                                while (placer != 1)//Placing y's
                                {
                                    int yx = random.Next(4, 55);
                                    int yy = random.Next(4, 25);
                                    if (area[yx, yy] != '#' && area[yx, yy] != 'X' && area[yx, yy] != 'Y')
                                    {
                                        area[yx, yy] = 'Y';
                                        placer = 1;
                                        Console.SetCursorPosition(yx, yy);
                                        Console.WriteLine("Y");
                                    }
                                    else
                                        placer = 0;
                                }
                            }
                            Console.ForegroundColor = ConsoleColor.Green;
                            for (int a = 0; a < 20; a++)
                            {
                                Console.ForegroundColor = ConsoleColor.Green;
                                placer = 0;
                                while (placer != 1)//Placing 1, 2 and 3 on screen.
                                {
                                    int ab = random.Next(4, 55);
                                    int ba = random.Next(4, 25);
                                    if (area[ab, ba] == ' ')
                                    {
                                        Console.SetCursorPosition(ab, ba);
                                        int randomize = random.Next(0, 10);
                                        if (randomize < 6)
                                        {
                                            area[ab, ba] = '1';
                                            Console.WriteLine("1");
                                        }
                                        else if (randomize < 9)
                                        {
                                            area[ab, ba] = '2';
                                            Console.WriteLine("2");
                                        }
                                        else
                                        {
                                            area[ab, ba] = '3';
                                            Console.WriteLine("3");
                                        }
                                        placer = 1;
                                    }
                                    else
                                        placer = 0;
                                }
                            }
                            Console.ForegroundColor = ConsoleColor.White;
                            int counter = 0, energy = 200, score = 0, mineamount = 50, mineplace = 0, collect1 = 0, collect2 = 0, collect3 = 0, collects = 0, pcount = 0, xcount = 0, ycount = 0;
                            string deathreason = "";
                            Console.SetCursorPosition(76, 5);
                            Console.WriteLine("0");
                            Console.SetCursorPosition(78, 7);
                            Console.WriteLine(energy);
                            Console.SetCursorPosition(77, 9);
                            Console.WriteLine(score);
                            Console.SetCursorPosition(83, 11);
                            Console.WriteLine(mineamount);
                            Console.ForegroundColor = ConsoleColor.Blue;
                            Console.SetCursorPosition(70, 13);
                            Console.WriteLine("START IN: ");
                            Console.ForegroundColor = ConsoleColor.DarkRed;
                            for (int i = 0; i < 3; i++)//Counts back from 3 to give time to player see around and strategize.
                            {
                                Console.SetCursorPosition(80, 13);
                                Console.WriteLine(3 - i);
                                Thread.Sleep(500);
                            }
                            Console.SetCursorPosition(70, 13);
                            Console.WriteLine("           ");
                            int mainlooper = 0, superpow = 2, superpowtime = 0;
                            while (mainlooper == 0)
                            {
                                if (Console.KeyAvailable)
                                {
                                    if (energy != 0)
                                    {
                                        cki = Console.ReadKey(true);
                                        Console.ForegroundColor = ConsoleColor.DarkYellow;
                                        int movementcheck = 0;
                                        if (cki.Key == ConsoleKey.RightArrow && area[px + 1, py] != '#' && area[px + 1, py] != 'X' && area[px + 1, py] != 'Y')//Moving to right
                                        {
                                            Console.SetCursorPosition(px, py);
                                            Console.WriteLine(" ");//Emptying the old location
                                            area[px, py] = ' ';
                                            px++;
                                            mineplace = 1;//To see where the mine will go if placed
                                            movementcheck = 1;
                                        }
                                        else if (cki.Key == ConsoleKey.LeftArrow && area[px - 1, py] != '#' && area[px - 1, py] != 'X' && area[px - 1, py] != 'Y')//Moving to left
                                        {
                                            Console.SetCursorPosition(px, py);
                                            Console.WriteLine(" ");
                                            area[px, py] = ' ';
                                            px--;
                                            mineplace = 2;
                                            movementcheck = 1;
                                        }
                                        else if (cki.Key == ConsoleKey.UpArrow && area[px, py - 1] != '#' && area[px, py - 1] != 'X' && area[px, py - 1] != 'Y')//Moving to up
                                        {
                                            Console.SetCursorPosition(px, py);
                                            Console.WriteLine(" ");
                                            area[px, py] = ' ';
                                            py--;
                                            mineplace = 3;
                                            movementcheck = 1;
                                        }
                                        else if (cki.Key == ConsoleKey.DownArrow && area[px, py + 1] != '#' && area[px, py + 1] != 'Y' && area[px, py + 1] != 'X')//Moving to down
                                        {
                                            Console.SetCursorPosition(px, py);
                                            Console.WriteLine(" ");
                                            area[px, py] = ' ';
                                            py++;
                                            mineplace = 4;
                                            movementcheck = 1;
                                        }
                                        else if (cki.Key == ConsoleKey.Spacebar)//Dropping a mine
                                        {
                                            if (mineamount > 0)//Checking mine amount
                                            {
                                                int ax = px; int ay = py;
                                                int mineifcheck = 0;
                                                if ((mineplace == 1) && (area[px - 1, py] != 'X') && (area[px - 1, py] != 'Y'))//Placing the mine if the area is empty
                                                {
                                                    ax -= 1;
                                                    mineifcheck++;
                                                }
                                                else if ((mineplace == 2) && (area[px + 1, py] != 'X') && (area[px + 1, py] != 'Y'))
                                                {
                                                    ax += 1;
                                                    mineifcheck++;
                                                }
                                                else if ((mineplace == 3) && (area[px, py + 1] != 'X') && (area[px, py + 1] != 'Y'))
                                                {
                                                    ay += 1;
                                                    mineifcheck++;
                                                }
                                                else if ((mineplace == 4) && (area[px, py - 1] != 'X') && (area[px, py - 1] != 'Y'))
                                                {
                                                    ay -= 1;
                                                    mineifcheck++;
                                                }
                                                if (mineifcheck == 1)
                                                {
                                                    Console.ForegroundColor = ConsoleColor.Cyan;
                                                    Console.SetCursorPosition(ax, ay);
                                                    area[ax, ay] = '+';
                                                    Console.WriteLine("+");
                                                    mineamount--;
                                                    Console.ForegroundColor = ConsoleColor.White;
                                                }
                                            }
                                        }
                                        else if (cki.Key == ConsoleKey.Escape) mainlooper = 1;
                                        if (movementcheck == 1)//If there was movement
                                        {
                                            if (area[px, py] == '1')
                                            {
                                                score += 10;
                                                eatsound.Play();
                                                collect1++;
                                            }
                                            else if (area[px, py] == '2')
                                            {
                                                score += 30;
                                                energy += 50;
                                                eatsound.Play();
                                                collect2++;
                                            }
                                            else if (area[px, py] == '3')
                                            {
                                                score += 90;
                                                energy += 200;
                                                eatsound.Play();
                                                mineamount++;
                                                collect3++;
                                            }
                                            else if (area[px, py] == 'S')
                                            {
                                                superpowtime += 100;
                                                eatsound.Play();
                                                collects++;
                                            }
                                            else if (area[px, py] == '+')//If player hits mine
                                            {
                                                Console.SetCursorPosition(px, py);
                                                Console.WriteLine(" ");
                                                minesound.Play();
                                                deathreason = "You hit a mine suicidal?";
                                                mainlooper = 1;//Closing the game and going to end screen
                                            }
                                            energy--;
                                            Console.SetCursorPosition(px, py);//Moving to new location
                                            Console.WriteLine("©");
                                            area[px, py] = '©';
                                        }
                                        Console.ForegroundColor = ConsoleColor.White;
                                    }
                                }
                                while (Console.KeyAvailable)
                                    Console.ReadKey(false);//Cleaning the keyboard buffer.
                                if (superpowtime > 0)
                                {
                                    Console.SetCursorPosition(70, 13);
                                    Console.ForegroundColor = ConsoleColor.DarkCyan;
                                    Console.Write("Super power time left:");
                                    Console.ForegroundColor = ConsoleColor.White;
                                    Console.WriteLine(" " + superpowtime / 5 + " ");
                                    superpow = 1;
                                    superpowtime -= 1;
                                }
                                else
                                {
                                    superpow = 2;
                                    Console.SetCursorPosition(70, 13);
                                    Console.WriteLine("                         ");
                                }
                                Console.ForegroundColor = ConsoleColor.DarkRed;
                                int energydependency = 1;
                                if (energy < 60)
                                    energydependency = 2;//2x movement speed for enemies if energy is low
                                int xymove = counter * superpow;
                                if (xymove % 2 == 0)
                                {
                                    for (int o = 0; o < energydependency; o++)
                                    {
                                        int[,] skippen = new int[56, 26];//To not make the same entity move twice.
                                        for (int i = 0; i < area.GetLength(0); i++)
                                        {
                                            for (int j = 0; j < area.GetLength(1); j++)
                                            {
                                                int movementer = 0;
                                                if (area[i, j] == 'X')
                                                    movementer = 1;
                                                if (area[i, j] == 'Y')
                                                    movementer = 2;//Determining if the entity is x or y
                                                int xmovementcheck = 0;
                                                xx = i;
                                                xy = j;
                                                if (movementer == 1)//If entity is x its priority is to move on x axis
                                                {
                                                    if ((px - xx > 0) && (area[xx + 1, xy] != '#') && (skippen[xx, xy] != 1) && (area[xx + 1, xy] != 'X') && (area[xx + 1, xy] != 'Y') && (xmovementcheck != 1))//Going to right
                                                    {
                                                        Console.SetCursorPosition(xx, xy);
                                                        Console.WriteLine(" ");
                                                        area[xx, xy] = ' ';
                                                        xx++;
                                                        xmovementcheck = 1;
                                                    }
                                                    else if ((px - xx < 0) && (area[xx - 1, xy] != '#') && (skippen[xx, xy] != 1) && (area[xx - 1, xy] != 'X') && (area[xx - 1, xy] != 'Y') && (xmovementcheck != 1))//Going to left
                                                    {
                                                        Console.SetCursorPosition(xx, xy);
                                                        Console.WriteLine(" ");
                                                        area[xx, xy] = ' ';
                                                        xx--;
                                                        xmovementcheck = 1;
                                                    }
                                                }
                                                if ((movementer == 1) || (movementer == 2))//If entity is y its priority is to move on y axis and if its x and didnt move in x axis it moves in y axis
                                                {
                                                    if ((py - xy > 0) && (area[xx, xy + 1] != '#') && (skippen[xx, xy] != 1) && (area[xx, xy + 1] != 'X') && (area[xx, xy + 1] != 'Y') && (xmovementcheck != 1))//Going down
                                                    {
                                                        Console.SetCursorPosition(xx, xy);
                                                        Console.WriteLine(" ");
                                                        area[xx, xy] = ' ';
                                                        xy++;
                                                        xmovementcheck = 1;
                                                    }
                                                    else if ((py - xy < 0) && (area[xx, xy - 1] != '#') && (skippen[xx, xy] != 1) && (area[xx, xy - 1] != 'X') && (area[xx, xy - 1] != 'Y') && (xmovementcheck != 1))//Going up
                                                    {
                                                        Console.SetCursorPosition(xx, xy);
                                                        Console.WriteLine(" ");
                                                        area[xx, xy] = ' ';
                                                        xy--;
                                                        xmovementcheck = 1;
                                                    }
                                                }
                                                if (movementer == 2)//if entity is x and didnt move in y axis it moves in x axis
                                                {
                                                    if ((px - xx > 0) && (area[xx + 1, xy] != '#') && (skippen[xx, xy] != 1) && (area[xx + 1, xy] != 'X') && (area[xx + 1, xy] != 'Y') && (xmovementcheck != 1))//Going to right
                                                    {
                                                        Console.SetCursorPosition(xx, xy);
                                                        Console.WriteLine(" ");
                                                        area[xx, xy] = ' ';
                                                        xx++;
                                                        xmovementcheck = 1;
                                                    }
                                                    else if ((px - xx < 0) && (area[xx - 1, xy] != '#') && (skippen[xx, xy] != 1) && (area[xx - 1, xy] != 'X') && (area[xx - 1, xy] != 'Y') && (xmovementcheck != 1))//Going to left
                                                    {
                                                        Console.SetCursorPosition(xx, xy);
                                                        Console.WriteLine(" ");
                                                        area[xx, xy] = ' ';
                                                        xx--;
                                                        xmovementcheck = 1;
                                                    }
                                                }
                                                if (xmovementcheck == 1)//If the entity moved
                                                {
                                                    Console.SetCursorPosition(xx, xy);//Placing to new location
                                                    if (area[xx, xy] == '+')//If hits a mine
                                                    {
                                                        area[xx, xy] = ' ';
                                                        Console.WriteLine(" ");
                                                        minesound.Play();
                                                        score += 300;
                                                    }
                                                    else if (movementer == 1)
                                                    {
                                                        Console.WriteLine("X");
                                                        if (area[xx, xy] == '©')//If player is eaten
                                                        {
                                                            deathreason = "X has gotten to you";
                                                            area[xx, xy] = 'X';
                                                            skippen[xx, xy] = 1;
                                                            mainlooper = 1;
                                                        }
                                                        area[xx, xy] = 'X';
                                                    }
                                                    else if (movementer == 2)
                                                    {
                                                        Console.WriteLine("Y");
                                                        if (area[xx, xy] == '©')//If player is eaten
                                                        {
                                                            deathreason = "You couldn't outrun Y";
                                                            area[xx, xy] = 'Y';
                                                            skippen[xx, xy] = 1;
                                                            mainlooper = 1;
                                                        }
                                                        area[xx, xy] = 'Y';
                                                    }
                                                    skippen[xx, xy] = 1;
                                                }
                                            }
                                        }
                                    }
                                }
                                Console.ForegroundColor = ConsoleColor.White;
                                for (int i = 0; i < area.GetLength(0); i++)
                                {
                                    for (int j = 0; j < area.GetLength(1); j++)
                                    {
                                        if (area[i, j] == 'S')
                                        {
                                            if (counter % 3 == 0)
                                                Console.ForegroundColor = ConsoleColor.DarkGreen;
                                            else if (counter % 3 == 1)
                                                Console.ForegroundColor = ConsoleColor.Magenta;
                                            else if (counter % 3 == 2)
                                                Console.ForegroundColor = ConsoleColor.DarkYellow;
                                            Console.SetCursorPosition(i, j);
                                            Console.WriteLine("S");
                                        }
                                        if (area[i, j] == 'X')
                                            xcount++;
                                        if (area[i, j] == 'Y')
                                            ycount++;
                                        if (area[i, j] == '©')//If player is gone pcount will be 0
                                            pcount++;
                                    }
                                }
                                if (pcount == 0)
                                    mainlooper = 1;//Goes to end screen
                                Thread.Sleep(200);
                                counter++;
                                Console.ForegroundColor = ConsoleColor.White;
                                if (counter % 5 == 0)//Writing the time on the table
                                {
                                    Console.SetCursorPosition(76, 5);
                                    Console.WriteLine(counter / 5);
                                }
                                Console.SetCursorPosition(77, 7);
                                Console.WriteLine(" " + energy + " ");
                                Console.SetCursorPosition(76, 9);
                                Console.WriteLine(" " + score + " ");
                                Console.SetCursorPosition(82, 11);
                                Console.WriteLine(" " + mineamount + " ");
                                if (counter % 10 == 0)//Placing 1, 2 or 3 every 2 seconds. 
                                {
                                    placer = 0;
                                    while (placer != 1)
                                    {
                                        Console.ForegroundColor = ConsoleColor.Green;
                                        int ab = random.Next(4, 55);
                                        int ba = random.Next(4, 25);
                                        if (area[ab, ba] == ' ')
                                        {
                                            int speedupchance = random.Next(1, 11);
                                            Console.SetCursorPosition(ab, ba);
                                            if (speedupchance < 10)
                                            {
                                                int randomize = random.Next(0, 10);
                                                if (randomize < 6)
                                                {
                                                    area[ab, ba] = '1';
                                                    Console.WriteLine("1");
                                                }
                                                else if (randomize < 9)
                                                {
                                                    area[ab, ba] = '2';
                                                    Console.WriteLine("2");
                                                }
                                                else
                                                {
                                                    area[ab, ba] = '3';
                                                    Console.WriteLine("3");
                                                }
                                            }
                                            else
                                            {
                                                area[ab, ba] = 'S';
                                                Console.ForegroundColor = ConsoleColor.DarkYellow;
                                                Console.WriteLine("S");
                                            }
                                            placer = 1;
                                        }
                                        else
                                            placer = 0;
                                    }
                                    Console.ForegroundColor = ConsoleColor.DarkRed;
                                }
                                if (counter % 150 == 0)//Placing 1 x and 1 y every 30 seconds.
                                {
                                    placer = 0;
                                    while (placer != 1)
                                    {
                                        int checky = 0;
                                        xx = random.Next(4, 55);
                                        xy = random.Next(4, 25);
                                        if (area[xx, xy] != '#')
                                            checky = 1;
                                        else
                                            placer = 0;
                                        int yx = random.Next(4, 55);
                                        int yy = random.Next(4, 25);
                                        if (area[yx, yy] != '#' && checky == 1)
                                        {
                                            area[yx, yy] = 'Y';
                                            placer = 1;
                                            Console.SetCursorPosition(yx, yy);
                                            Console.WriteLine("Y");
                                            area[xx, xy] = 'X';
                                            Console.SetCursorPosition(xx, xy);
                                            Console.WriteLine("X");
                                        }
                                        else
                                            placer = 0;
                                    }
                                }
                                int wallmakey = 0;
                                while (wallmakey == 0)//Wall changes
                                {
                                    int topwall = 0, bottomwall = 0, rightwall = 0, leftwall = 0, controller = 0;
                                    int wallx = random.Next(1, 11);
                                    int wally = random.Next(1, 5);//Picking a core from the top left corner.
                                    int pickedwall = random.Next(1, 5);
                                    if (area[wallx * 5 + 1, wally * 5] == '#')//Checking the walls and making 1 if htere is wall.
                                        topwall = 1;
                                    if (area[wallx * 5, wally * 5 + 1] == '#')
                                        leftwall = 1;
                                    if (area[wallx * 5 + 1, wally * 5 + 3] == '#')
                                        bottomwall = 1;
                                    if (area[wallx * 5 + 3, wally * 5 + 1] == '#')
                                        rightwall = 1;
                                    wallx *= 5; wally *= 5;//So its easier to write and read the code.
                                    if (pickedwall == 1 && (area[wallx, wally] == ' ' || area[wallx, wally] == '#') && (area[wallx + 1, wally] == ' ' || area[wallx + 1, wally] == '#') && (area[wallx + 2, wally] == ' ' || area[wallx + 2, wally] == '#') && (area[wallx + 3, wally] == ' ' || area[wallx + 3, wally] == '#') && area[wallx + 3, wally] != 'X' && area[wallx + 3, wally] != 'Y' && area[wallx + 3, wally] != '©')
                                    {
                                        topwall = (topwall + 1) % 2;//Makes it 0 if its 1 and 1 if its 0.
                                        controller = 1;
                                    }
                                    if (pickedwall == 2 && (area[wallx + 3, wally] == ' ' || area[wallx + 3, wally] == '#') && (area[wallx + 3, wally + 1] == ' ' || area[wallx + 3, wally + 1] == '#') && (area[wallx + 3, wally + 2] == ' ' || area[wallx + 3, wally + 2] == '#') && (area[wallx + 3, wally + 3] == ' ' || area[wallx + 3, wally + 3] == '#') && area[wallx + 3, wally + 3] != 'X' && area[wallx + 3, wally + 3] != 'Y' && area[wallx + 3, wally + 3] != '©')
                                    {
                                        rightwall = (rightwall + 1) % 2;
                                        controller = 1;
                                    }
                                    if (pickedwall == 3 && (area[wallx, wally + 3] == ' ' || area[wallx, wally + 3] == '#') && (area[wallx + 1, wally + 3] == ' ' || area[wallx + 1, wally + 3] == '#') && (area[wallx + 2, wally + 3] == ' ' || area[wallx + 2, wally + 3] == '#') && (area[wallx + 3, wally + 3] == ' ' || area[wallx + 3, wally + 3] == '#') && area[wallx + 3, wally + 3] != 'X' && area[wallx + 3, wally + 3] != 'Y' && area[wallx + 3, wally + 3] != '©')
                                    {
                                        bottomwall = (bottomwall + 1) % 2;
                                        controller = 1;
                                    }
                                    if (pickedwall == 4 && (area[wallx, wally] == ' ' || area[wallx, wally] == '#') && (area[wallx, wally + 1] == ' ' || area[wallx, wally + 1] == '#') && (area[wallx, wally + 2] == ' ' || area[wallx, wally + 2] == '#') && (area[wallx, wally + 3] == ' ' || area[wallx, wally + 3] == '#') && area[wallx, wally + 3] != 'X' && area[wallx, wally + 3] != 'Y' && area[wallx, wally + 3] != '©')
                                    {
                                        leftwall = (leftwall + 1) % 2;
                                        controller = 1;
                                    }
                                    if (controller == 1)
                                    {
                                        if (topwall + leftwall + bottomwall + rightwall == 4 || topwall + leftwall + bottomwall + rightwall == 0)//If there will be 4 or 0 walls rerolls.
                                            continue;
                                        else
                                        {
                                            if (pickedwall == 1)
                                            {
                                                for (int i = 0; i < 4; i++)
                                                {
                                                    if (topwall == 0)//Deleting the wall
                                                        area[wallx + i, wally] = ' ';
                                                    if (topwall == 1)//Adding wall
                                                        area[wallx + i, wally] = '#';
                                                }
                                                if (leftwall == 1)//If there is wall there the corner will still have wall.
                                                    area[wallx, wally] = '#';
                                                if (rightwall == 1)
                                                    area[wallx + 3, wally] = '#';
                                            }
                                            else if (pickedwall == 2)
                                            {
                                                for (int i = 0; i < 4; i++)
                                                {
                                                    if (rightwall == 0)
                                                        area[wallx + 3, wally + i] = ' ';
                                                    if (rightwall == 1)
                                                        area[wallx + 3, wally + i] = '#';
                                                }
                                                if (topwall == 1)
                                                    area[wallx + 3, wally] = '#';
                                                if (bottomwall == 1)
                                                    area[wallx + 3, wally + 3] = '#';
                                            }
                                            else if (pickedwall == 3)
                                            {
                                                for (int i = 0; i < 4; i++)
                                                {
                                                    if (bottomwall == 0)
                                                        area[wallx + i, wally + 3] = ' ';
                                                    if (bottomwall == 1)
                                                        area[wallx + i, wally + 3] = '#';
                                                }
                                                if (leftwall == 1)
                                                    area[wallx, wally + 3] = '#';
                                                if (rightwall == 1)
                                                    area[wallx + 3, wally + 3] = '#';
                                            }
                                            else if (pickedwall == 4)
                                            {
                                                for (int i = 0; i < 4; i++)
                                                {
                                                    if (leftwall == 0)
                                                        area[wallx, wally + i] = ' ';
                                                    if (leftwall == 1)
                                                        area[wallx, wally + i] = '#';
                                                }
                                                if (topwall == 1)
                                                    area[wallx, wally] = '#';
                                                if (bottomwall == 1)
                                                    area[wallx, wally + 3] = '#';
                                            }
                                            for (int i = 0; i < 4; i++)//Writing the changed wall (all of that core)
                                            {
                                                for (int j = 0; j < 4; j++)
                                                {
                                                    if ((i == 1 || i == 2) && (j == 1 || j == 2))//To not write inside of the cores again
                                                        continue;
                                                    else
                                                    {
                                                        Console.BackgroundColor = ConsoleColor.Black;//Making the colours
                                                        if (area[wallx + i, wally + j] == '#')
                                                        {
                                                            Console.ForegroundColor = ConsoleColor.DarkBlue;
                                                            Console.BackgroundColor = ConsoleColor.DarkBlue;
                                                        }
                                                        else if (area[wallx + i, wally + j] == '1' || area[wallx + i, wally + j] == '2' || area[wallx + i, wally + j] == '3')
                                                            Console.ForegroundColor = ConsoleColor.DarkGreen;
                                                        else if (area[wallx + i, wally + j] == 'X' || area[wallx + i, wally + j] == 'Y')
                                                            Console.ForegroundColor = ConsoleColor.DarkRed;
                                                        else if (area[wallx + i, wally + j] == '+')
                                                            Console.ForegroundColor = ConsoleColor.Cyan;
                                                        else if (area[wallx + i, wally + j] == '©')
                                                            Console.ForegroundColor = ConsoleColor.DarkYellow;
                                                        Console.SetCursorPosition(wallx + i, wally + j);
                                                        Console.WriteLine(area[wallx + i, wally + j]);
                                                    }
                                                }
                                            }
                                            wallmakey = 1;
                                        }
                                    }
                                }
                                Console.ForegroundColor = ConsoleColor.White;
                                Console.BackgroundColor = ConsoleColor.Black;
                            }
                            Thread.Sleep(1000);
                            while (Console.KeyAvailable)
                                Console.ReadKey(false);//Cleaning the keyboard buffer.
                            deadsound.Play();
                            string top5 = "Not ranked";
                            if (score > maxscore[4])//Adding to highest scores.
                            {
                                saveline[saveline.Length - 1] = Convert.ToString(score);
                                top5 = "Made a place in ranking";
                            }
                            using (StreamWriter writer = new StreamWriter("highestscore.txt"))
                            {
                                for (int i = 0; i < saveline.Length; i++)
                                {
                                    if (saveline[i] != "a")//If its not a new maximum not writing cuz it results with a mistake
                                        writer.WriteLine(saveline[i]);
                                }
                            }
                            Console.Clear();
                            if (score < 2500)//Making the end screen
                                Console.ForegroundColor = ConsoleColor.DarkRed;
                            else
                                Console.ForegroundColor = ConsoleColor.DarkGreen;
                            Console.SetCursorPosition(50, 10);
                            Console.WriteLine("GAME OVER");
                            Console.SetCursorPosition(44, 12);
                            Console.ForegroundColor = ConsoleColor.DarkYellow;
                            Console.WriteLine(deathreason);
                            Console.ForegroundColor = ConsoleColor.DarkRed;
                            Console.SetCursorPosition(50, 14);
                            Console.WriteLine("Score: " + score);
                            Console.SetCursorPosition(50, 16);
                            Console.WriteLine("Time: " + counter / 5);
                            Console.SetCursorPosition(22, 18);
                            Console.WriteLine("Press A to play again, Tab to see inspect, Escape to exit to main menu");
                            int checknext = 0;
                            while (checknext == 0)
                            {
                                if (Console.KeyAvailable)
                                {
                                    cki = Console.ReadKey(true);
                                    if (cki.Key == ConsoleKey.Escape)
                                    {
                                        menureturner = 1;
                                        checknext = 1;
                                        Console.Clear();
                                        Console.ForegroundColor = ConsoleColor.Cyan;
                                        Console.SetCursorPosition(10, 0);
                                        Console.WriteLine(sa);
                                    }
                                    else if (cki.Key == ConsoleKey.A)
                                        checknext = 1;
                                    else if (cki.Key == ConsoleKey.Tab)
                                    {
                                        Console.Clear();
                                        checknext = 1;
                                        menureturner = 1;
                                        while (inspection == 0)
                                        {
                                            Console.ForegroundColor = ConsoleColor.DarkYellow;
                                            Console.SetCursorPosition(10, 4);
                                            Console.WriteLine("Collected 1, 2, 3 and S amounts: " + collect1 + ", " + collect2 + ", " + collect3 + ", " + collects);
                                            Console.SetCursorPosition(10, 6);
                                            Console.WriteLine("Energy spent: " + (200 + 200 * collect3 + 50 * collect2 - energy));
                                            Console.SetCursorPosition(10, 8);
                                            Console.WriteLine("Killed X amount: " + (2 + (counter / 150) - xcount / counter));
                                            Console.SetCursorPosition(10, 10);
                                            Console.WriteLine("Killed Y amount: " + (2 + (counter / 150) - ycount / counter));
                                            Console.SetCursorPosition(10, 12);
                                            Console.WriteLine("Time spent: " + counter / 5);
                                            Console.SetCursorPosition(10, 14);
                                            Console.WriteLine("Score: " + score);
                                            Console.SetCursorPosition(10, 16);
                                            Console.WriteLine("Death reason: " + deathreason);
                                            Console.SetCursorPosition(10, 18);
                                            Console.WriteLine("Ranking: " + top5);
                                            Console.SetCursorPosition(0, 20);
                                            Console.WriteLine("Press enter to countinue");
                                            if (Console.KeyAvailable)
                                            {
                                                cki = Console.ReadKey(true);
                                                if (cki.Key == ConsoleKey.Enter)//Back to main menu
                                                {
                                                    Console.Clear();
                                                    inspection = 1;
                                                    Console.ForegroundColor = ConsoleColor.Cyan;
                                                    Console.SetCursorPosition(10, 0);
                                                    Console.WriteLine(sa);
                                                }
                                            }
                                        }
                                    }
                                    else
                                    {
                                        Console.SetCursorPosition(0, 20);
                                        Console.WriteLine("Please press A, Tab or Esc");
                                    }
                                }
                            }
                        }
                    }
                    else if (cki.Key == ConsoleKey.L)//Print leaderboard
                    {
                        Console.Clear();
                        for (int i = 0; i < 5; i++)
                        {
                            if (i == 0)
                                Console.ForegroundColor = ConsoleColor.DarkYellow;
                            else if (i == 1)
                                Console.ForegroundColor = ConsoleColor.DarkGray;
                            else if (i == 2)
                                Console.ForegroundColor = ConsoleColor.Green;
                            else
                                Console.ForegroundColor = ConsoleColor.DarkCyan;
                            Console.SetCursorPosition(54, 10 + 2 * i);
                            Console.WriteLine(i + 1 + ". " + maxscore[i]);//Printing the highest scores with 1 line gap
                        }
                        Console.ForegroundColor = ConsoleColor.DarkGreen;
                        Console.SetCursorPosition(0, 20);
                        Console.WriteLine("Press Enter to continue");
                        int checknext = 0;
                        while (checknext == 0)
                        {
                            if (Console.KeyAvailable)
                            {
                                cki = Console.ReadKey(true);
                                if (cki.Key == ConsoleKey.Enter)//Back to main menu
                                {
                                    Console.Clear();
                                    checknext = 1;
                                    Console.ForegroundColor = ConsoleColor.Cyan;
                                    Console.SetCursorPosition(10, 0);
                                    Console.WriteLine(sa);
                                }
                                else
                                {
                                    Console.SetCursorPosition(0, 20);
                                    Console.WriteLine("Press Enter to continue");
                                }
                            }
                        }
                    }
                    else if (cki.Key == ConsoleKey.Escape)//Closes the game
                    {
                        Console.Clear();
                        break;
                    }
                }
            }
        }
    }
}


