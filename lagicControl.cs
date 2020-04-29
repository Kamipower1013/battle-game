using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Diagnostics;
namespace rpg
{
    class logicControl
    {
        static Random random = new Random();
        public const int Width = 30;
        public const int Height = 30;
        public static string[,] buffer = new string[Width, Height];
        //public static ConsoleCanvasString Canvas = new ConsoleCanvasString(Width, Height);
        public static Player player = new Player("雾亥", 3000, 1000, 100, 25, 40, 3, 3, 1, 1000);
        public static under_canvasbattle under_Canvasbattle = new under_canvasbattle();
        public static List<Monster> monsterlist = new List<Monster>();
        public static List<NPC> npcslist = new List<NPC>();
        public static Skill normal_skill = new Skill("斩击", 0, 80, 0, 0, 0);
        public static Skill overtime_skill = new Skill("抵近射击", 1, 20, 3, 0, 5);
        public static Skill cure_myself = new Skill("纳米供给血氧泵", 2, -60, 0, 0, 10);
        public static Skill execute_skill = new Skill("拟态重力子炮", 3, 0, 0, 200, 10);
        public static NPC npc = new NPC("造换塔控制台", 0, Width - 3, Height - 3);
        public static Gate gate = new Gate("构造传送塔", 0, 3, Height - 3);
        static public string mission_target = "";
        static public string massage_below = "";
        static public string[,] gravity_weapon = new string[2, 5];
        public static bool game_over;
        public static bool victory;
        
        
        static public void create_weapon()//用于显示大招效果，已弃用
        {
            for (int i = player.pos.x; i < gravity_weapon.GetLength(0); i++)
            {
                for (int j = player.pos.y; j < gravity_weapon.GetLength(1); j++)
                {
                    gravity_weapon[i, j] = "■";

                }


            }

        }
        static public  void create_weapon_destoryemeny()//配合大招的动画效果已弃用
        {     
                            for (int k = 0; k < monsterlist.Count; k++)
                             {
                                 for (int i = player.pos.x-5; i < player.pos.x+5; i++)
                                  {
                                    for (int j = player.pos.y-5; j <player.pos.y+5; j++)
                                    {
                                        if (monsterlist[k].pos.x==j||monsterlist[k].pos.y == i)
                                                {     
                                                        player.Levelupdate(monsterlist[k]);
                                                        monsterlist.Remove(monsterlist[k]);
                                                }
                                     }

                                 }
              

                            }             
        }
        public static Pos RandPos()//用于随机生成人物位置
        {
            while (true)
            {
                Pos pos=new Pos(0,0);
                pos.x= random.Next(2,Width-2);
                pos.y = random.Next(2, Height -2);
                
                if (buffer[pos.x,pos.y] =="  ")
                {
                    return pos;
                }
            }
        }

        static public void Create_mapedge()//创建边框
        {
            for (int i = 0; i < buffer.GetLength(0); i++)
            {
                buffer[0, i] = "##";
                buffer[Height - 1, i] = "##";
            }
            for (int i = 0; i < buffer.GetLength(1); i++)
            {
                buffer[i, 0] = "##";
                buffer[i, Width - 1] = "##";
            }

        }
        static public void Clear_map()//刷新函数之一
        {
            for (int i = 0; i < buffer.GetLength(0); i++)
            {
                for (int j = 0; j < buffer.GetLength(1); j++)
                {
                    buffer[i, j] = "  ";
                }
            }
        }

        static public void DrawOther()
        {
           
            buffer[player.pos.x,player.pos.y] ="雾";
            for(int i = 0; i < monsterlist.Count; i++)
            {   if (monsterlist[i].name == "硅基生物")
                    buffer[monsterlist[i].pos.x, monsterlist[i].pos.y] = "硅";
                else if(monsterlist[i].name == "网络安全警卫")
                {
                    buffer[monsterlist[i].pos.x, monsterlist[i].pos.y] = "警";
                }else if(monsterlist[i].name == "高级安全警卫娜塔莎")
                {
                    buffer[monsterlist[i].pos.x, monsterlist[i].pos.y] = "娜";
                }
            }
            for (int i = 0; i < npcslist.Count; i++)
            {
                if (npcslist[i].name == "造换塔控制台")
                {
                    buffer[npcslist[i].pos.x, npcslist[i].pos.y] = "台";
                }
                else if(npcslist[i].name == "构造传送塔")
                {
                    buffer[npcslist[i].pos.x, npcslist[i].pos.y] = "门";
                }
                    
            }
        }

        static public void Draw_all()
        {
            Clear_map();
            Create_mapedge();
            DrawOther();
            Refresh();
        }
        

        public static void GameOver()
        {
            game_over = true;
            victory = false;
        }

        public static void OnStageClear()
        {
            game_over = true;
            victory = true;
        }
        static void ClearStage()
        {  
            game_over = false;
            victory = false;
            player.PosReset();
            mission_target = "";
            massage_below = "";

        }
        public bool IsPosEmpty(Pos pos)
        {
            if (pos.x < 1 || pos.y < 1 || pos.x >Width-1 || pos.y >Height-1)
            {
                return false;
            }
            for(int i=0;i<monsterlist.Count;i++)
            {   if(pos.x==monsterlist[i].pos.x&&pos.y==monsterlist[i].pos.y)
                return false;
            }
            for (int i = 0; i <npcslist.Count; i++)
            {
                if (pos.x == npcslist[i].pos.x && pos.y ==npcslist[i].pos.y)
                    return false;
            }

            if (pos.x == player.pos.x && pos.y ==player.pos.y)
            {
                return false;
            }
            return true;
        }


        static public void drawinfo(Player player)
        {
            //string s = string.Format("hp:{0}level:{1}atk:{2}$:{3}exp:{4}", player.hp, player.level, player.base_attackValue, player.money, player.exp);

            //for (int i = 0; i < s.Length; ++i)
            //{
            //    buffer[0, i] = Convert.ToString(s[i]);
            //}
            Console.WriteLine("=============================");
            Console.WriteLine("姓名:{0}          生命值:{1}", player.name, player.hp);
            Console.WriteLine("炭基电池电量:{0}  攻击力:{1}", player.mp, player.base_attackValue);
            Console.WriteLine("防御:{0}          闪避:{1}", player.defense, player.dex);
            Console.WriteLine("等级:{0}          经验值:{1}", player.level, player.exp);
            Console.WriteLine("无机及有机材料:{0}", player.money);
            Console.WriteLine("=============================");


            //for(int i = 0; i < player.itemList.Count; i++)
            //{

            //}

        }
        static public void drawbattleinfo()
        {
            Console.WriteLine(massage_below);
  
        }
       
        static void Refresh()
        {
            Console.Clear();
            drawinfo(player);
            for (int i = 0; i < buffer.GetLength(0); i++)
            {
                for (int j = 0; j < buffer.GetLength(1); j++)
                {
                    Console.Write(buffer[i, j]);
                }
                Console.WriteLine();
            }
            drawbattleinfo();
        }


        static void Jud_MovePlayer()//移动角色
        {
            if (player.pos.x >1 &&player.pos.x <= Width - 1 && player.pos.y >1 && player.pos.y <=Height - 1)
            {

                ConsoleKeyInfo key = Console.ReadKey(true);

                if (key.Key == ConsoleKey.UpArrow)
                {
                    player.pos.x -= 1;
                }
                else if (key.Key == ConsoleKey.DownArrow)
                {
                    player.pos.x += 1;
                }
                else if (key.Key == ConsoleKey.LeftArrow)
                {
                    player.pos.y -= 1;
                }
                else if (key.Key == ConsoleKey.RightArrow)
                {
                    player.pos.y+= 1;
                }
                else
                {
                    massage_below += "无法行动\n";
                }
            }
            if (player.pos .x< 1 || player.pos.x >= Width - 1 || player.pos.y < 1 || player.pos.y >= Height - 1)
            {
                massage_below += "无法行动！\n";
            }

            //int next_pos = MapPos(next_x, next_y); 
         
            for (int i = 0; i < monsterlist.Count; i++)
            {
               
                if (player.pos.x == monsterlist[i].pos.x && player.pos.y == monsterlist[i].pos.y)
                {
                    
                    //massage_below = "遭遇" + monsterlist[i].name + "开始战斗：1、斩击,3、纳米供给血氧泵4、拟态重力子炮\n";
                    Console.WriteLine("遭遇{0}开始战斗：1、斩击,3、纳米供给血氧泵4、拟态重力子炮", monsterlist[i].name);
                    string c = Console.ReadLine();
                    int input = int.Parse(c);
                    while (input != 1 && input != 3 && input != 4)
                    {
                        //massage_below += "输入错误请重新输入\n";
                        Console.WriteLine("输入错误请重新输入");
                        string b = Console.ReadLine();
                        input = int.Parse(b);

                    }
                   
                    while (player.hp > 0 && monsterlist[i].hp > 0)
                    {
                        under_canvasbattle.Roundbattle_set(input, player,monsterlist[i], ref massage_below);
                        input = -1;
                        //massage_below += "开始战斗：1、斩击,3、纳米供给血氧泵4、拟态重力子炮\n";
                        Console.WriteLine("继续开始战斗：1、斩击,3、纳米供给血氧泵4、拟态重力子炮");
                        string b = Console.ReadLine();
                        input = int.Parse(b);
                        while (input != 1 && input != 3 && input != 4)
                        {
                            //massage_below += "输入错误请重新输入\n";
                            Console.WriteLine("输入错误请重新输入");
                            string k= Console.ReadLine();
                            input = int.Parse(k);

                        }
                    }
                    if (player.hp>0)
                    {
                        player.Levelupdate(monsterlist[i]);
                        monsterlist.Remove(monsterlist[i]);
                        
     
                    }
                    else if (monsterlist[i].hp > 0)
                    {
                     
                        break;
                    }

                }

             
            }

            for (int i = 0; i < npcslist.Count; i++)
            {    if (npcslist[i].name == "造换塔控制台")
                {
                    if (player.pos.x == npcslist[i].pos.x && player.pos.y == npcslist[i].pos.y)
                    {

                        string s;
                        npcslist[i].OnTalk(player, out s);
                        Console.WriteLine(s);
                        string c = Console.ReadLine();
                        int input = int.Parse(c);
                        while (input != 1 && input != 2 && input != 3 && input != 4)
                        {
                            //massage_below += "输入错误请重新输入\n";
                            Console.WriteLine("输入错误请重新输入");
                            string b = Console.ReadLine();
                            input = int.Parse(b);

                        }
                        if (player.money >= npcslist[i].itemlist[input - 1].cost)
                        {
                            player.base_attackValue += npcslist[i].itemlist[input - 1].add_baseattack;
                            player.hp += npcslist[i].itemlist[input - 1].add_Hp;
                            player.defense += npcslist[i].itemlist[input - 1].add_defense;
                            player.dex += npcslist[i].itemlist[input - 1].add_dex;
                            player.money -= npcslist[i].itemlist[input - 1].cost;
                            //Console.WriteLine("购买成功！");
                            massage_below += "购买成功\n";
                        }
                        else
                        {
                            massage_below += "购买失败\n";
                            //Console.WriteLine("购买成功！");

                        }
                    }


                }
                else if(npcslist[i].name == "构造传送塔")
                {
                    if (player.pos.x == npcslist[i].pos.x && player.pos.y == npcslist[i].pos.y)
                    {
                        string s;
                        npcslist[i].OnTalk(player, out s);
                        massage_below += s;
                    }
                }


            }
           
        }
          
        static public void level_1()
        {
            NPC npc1 = new NPC("造换塔控制台", 0, Width - 4, Height -4);
            npcslist.Add(npc);
            npcslist.Add(gate);
            Monster monster1 = Monster.CreateSi_enemy();
            Monster monster2 = Monster.create_safeguard();
            Monster monster3 = Monster.CreateSi_enemy();
            Monster monster4 = Monster.create_safeguard();
            monsterlist.Add(monster1);
            monsterlist.Add(monster2);
            monsterlist.Add(monster3);
            monsterlist.Add(monster4);
            massage_below = "击败所有安全警卫和硅素生物\n";
        }
        static public void level_2()
        {   massage_below= "";
            NPC npc1 = new NPC("造换塔控制台", 0, Width - 4, Height - 4);
            npcslist.Add(npc);
            Monster monster1 = Monster.CreateSi_enemy();
            Monster monster2 = Monster.create_safeguard();
            Monster monster3 = Monster.CreateSi_enemy();
            Monster monster4 = Monster.create_safeguard();
            Monster monster5 = Monster.create_BOSS();
            monsterlist.Add(monster1);
            monsterlist.Add(monster2);
            monsterlist.Add(monster3);
            monsterlist.Add(monster4);
            monsterlist.Add(monster5);
            massage_below = "击败所有安全警卫和硅素生物迎战高级安全警卫娜塔莎\n";
        }


        static public void level_3()
        {
            massage_below = "";
            NPC npc1 = new NPC("造换塔控制台", 0, Width - 4, Height - 4);
            npcslist.Add(npc);
            npcslist.Add(gate);
            Monster monster1 = Monster.CreateSi_enemy();
            Monster monster2 = Monster.create_safeguard();
            Monster monster3 = Monster.CreateSi_enemy();
            Monster monster4 = Monster.create_safeguard();
            monsterlist.Add(monster1);
            monsterlist.Add(monster2);
            monsterlist.Add(monster3);
            monsterlist.Add(monster4);
            massage_below = "击败所有安全警卫和硅素生物\n";
        }

        public  static void enemy_movement()
        {
            for(int i = 0; i < monsterlist.Count; i++)
            {
                if (monsterlist[i].pos.x < player.pos.x)
                {
                    monsterlist[i].pos.x++;
                }
                else if (monsterlist[i].pos.y <player.pos.y)
                {
                    monsterlist[i].pos.y++;
                }
                else if(monsterlist[i].pos.x > player.pos.x)
                {
                    monsterlist[i].pos.x--;
                }
                else if(monsterlist[i].pos.y > player.pos.y)
                {
                    monsterlist[i].pos.y--;
                }
            }
        }
        static void Main(string[] args)
        {
            player.skillList.Add(0,normal_skill);
            player.skillList.Add(2, cure_myself);
            player.skillList.Add(3, execute_skill);

            Clear_map();
            level_1();
            Create_mapedge();
            DrawOther();
            Draw_all();
            while (true)
            {
                Jud_MovePlayer();
                Draw_all();
               
                if (player.hp<=0|| (monsterlist.Count == 0 && player.pos.x == gate.pos.x && gate.pos.y == player.pos.y))
                {
                    break;
                }
            }
            if (player.hp > 0)
            {
                Clear_map();
                level_3();
                DrawOther();
                Draw_all();
                while (true)
                {
                    Jud_MovePlayer();
                    enemy_movement();
                    Draw_all();
                    if (monsterlist.Count > 0)
                    { 
                        Console.WriteLine("是否使用重力子射线装置?按y或n");
                        string fire = Console.ReadLine();
                        while (fire != "y" && fire != "n")
                        {
                            Console.WriteLine("输入错误，按y或n");
                            fire = Console.ReadLine();
                        }
                        if (fire == "y")
                        {
                            for (int i = 0; i < monsterlist.Count; i++)
                            {
                                player.Levelupdate(monsterlist[i]);

                            }
                            monsterlist.Clear();
                        }
                    }
                    if (player.hp <= 0 || (monsterlist.Count == 0 && player.pos.x == gate.pos.x && gate.pos.y == player.pos.y))
                    {
                        break;
                    }
                }
            }
            if (player.hp > 0)
            {
                Clear_map();
                level_2();
                Create_mapedge();
                DrawOther();
                Draw_all();
                while (true)
                {
                    Jud_MovePlayer();
                    Draw_all();

                    if ((player.hp <= 0) ||monsterlist.Count == 0)
                    {
                        break;
                    }
                }
                if(player.hp > 0)
                {
                    Console.WriteLine("你使超构造体恢复了秩序");
                }


            }
            else
            {
                Console.WriteLine("游戏结束！");
            }

            Console.ReadKey();
        }

    }                               
}
