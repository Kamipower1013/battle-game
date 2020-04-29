using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
namespace rpg
{
    class Player : Role
    {
        public int level;
        public int exp=0;
        public int money;
        public Player(string name, int hp, int mp, int base_attackValue,  int defense, int dex,int x,int y,int level,int money) : base(name, hp, mp, base_attackValue,defense, dex,x,y)
        {
            this.level = level;
            this.money = money;
        }
        public void Levelupdate(Monster monster)
        {   
            int exp_advancepoint = 100+(level-1)*100;
            if(monster.name== "网络安全警卫")
            {
                this.exp += 100;
                this.money += 300;
            }
            else if(monster.name == "硅基生物")
            {
                this.exp += 100;
                this.money += 100;
            }
            if (exp >= exp_advancepoint)
            {
                level++;
                dex = dex + 10;
                base_attackValue = base_attackValue + 60;
                exp = 0;
            }
        }
        public override void use_skill(Player player, Monster monster, int skillType, ref string battleinfo)
        {
            if (player.skillList.ContainsKey(skillType))
            {
                if (skillType == 0)
                {
                    monster.hp = monster.hp + monster.defense - player.skillList[0].damage-player.base_attackValue;
                  
                }
                else if (skillType == 2&&player.mp>player.skillList[2].mpcost)
                {
                    player.hp = player.hp + player.skillList[2].damage;
                    player.mp = player.mp - player.skillList[2].mpcost;
                }
                else if(skillType == 2 && player.mp <player.skillList[2].mpcost)
                {
                    Console.WriteLine("血氧泵数量不足！");
                }
                else if (skillType == 3)
                {
                    if (monster.hp < 200)
                    {
                        monster.hp = 0;
                    }
                    else
                    {
                        Console.WriteLine("敌方生命值不可能斩杀");
                    }
                }
                //else if (skillType == 1)
                //{   new
                //    states.Add()
                //}
            }
        }
        public void PosReset()
        {
            pos.x = 2;
            pos.x = 2;
           
        }

    }
    class Monster : Role
    {
        static Random randpos = new Random();
        //public int exp_drow;
        //public int money_drow;
        public Monster(string name, int hp, int mp, int base_attackValue ,int defense, int dex,int x,int y) : base(name, hp, mp,base_attackValue, defense, dex,x,y)
        {

        }
        static public Monster CreateSi_enemy()
        {
            Pos pos = logicControl.RandPos();
            Monster monster = new Monster("硅基生物",100,100,0,20,15,pos.x,pos.y);
            Skill skill = new Skill("硅素突刺", 0, 80, 0, 0,0);
            monster.skillList.Add(0, skill);
            
            return monster;
        }
        static  public Monster create_safeguard()
        {
            Pos pos = logicControl.RandPos();
            Monster monster = new Monster("网络安全警卫",300,100,0,20,20,pos.x,pos.y);
            Skill skill = new Skill("快速斩击", 0, 80, 0, 0,0);
            monster.skillList.Add(0, skill);
            return monster;

        }

        static public Monster create_BOSS()
        {    
            Monster monster = new Monster("高级安全警卫娜塔莎", 1000, 1000, 0,25, 40,10,10);
            Skill skill = new Skill("重力子射线放出装置", 0, 200, 0, 0, 500);
            //Skill skill1 = new Skill("死亡之刃", 0, 120, 0, 0, 0);
            //monster.skillList.Add(0, skill1);
            monster.skillList.Add(0, skill);
            return monster;
        }


        public override void use_skill(Player player, Monster monster, int skillType,ref string battleinfo)
        {
            if (player.skillList.ContainsKey(skillType))
            {
                if (skillType == 0&&monster.mp>= monster.skillList[0].mpcost)
                {
                    player.hp = player.hp + player.defense - monster.skillList[0].damage;
                    monster.mp = monster.mp - monster.skillList[0].mpcost;
                }
                else if (skillType == 2)
                {
                    //player.hp = player.hp + player.skillList[2].damage;
                }
                else if (skillType == 3)
                {
                    if (monster.hp < 200)
                    {
                        monster.hp = 0;
                    }
                    else
                    {
                        //battleinfo += "敌方生命值不可能斩杀\n";
                        Console.WriteLine("敌方生命值不可能斩杀");
                    }
                }
                else if (skillType ==4&&monster.mp>=monster.skillList[4].mpcost)
                {   

                    player.hp = player.hp + player.defense - monster.skillList[4].damage;
                    monster.mp = monster.mp - monster.skillList[4].mpcost;
                }
                else
                {
                    Console.WriteLine("电池电量不足无法使用技能");
                }
            }
           


        }
    }




    class under_canvasbattle
    {
        static Random randomnum = new Random();
        static public void Roundbattle_set(int input, Player player, Monster monster,ref string battleinfo)
        {
            
            if (player.hp > 0 && monster.hp > 0)
            {
                int first_attack = randomnum.Next(1, 101);//先攻判断
                int hero_slip = randomnum.Next(1, 101);//闪避判定
                int devil_slip = randomnum.Next(1, 101);//闪避判定
                //int devil_skill = randomnum.Next(1, 101);
                ///////////////////魔王先攻
                if (first_attack > 50)
                {
                    //battleinfo += "此回合先攻的是" + monster.name + "!\n";
                    //Console.WriteLine(battleinfo);
                    Console.WriteLine("此回合先攻的是{0}！",monster.name);
                    if (hero_slip > player.dex)
                      {
                        Console.WriteLine("{0}使用了{1}!", monster.name, monster.skillList[0].skillname);
                        monster.use_skill(player, monster,0,ref battleinfo);
                        //battleinfo += monster.name + "使用了" + monster.skillList[0].skillname;
                        //Console.WriteLine(battleinfo);
                       
                      }
                      else
                      {
                        //battleinfo += player.name + "闪避成功!\n";
                        Console.WriteLine("{0}使用了{1}!{2}闪避成功!", monster.name, monster.skillList[0].skillname, player.name);
                    }
                 
                   
                    if (player.hp > 0 && input == 1)
                    {

                        if (devil_slip > monster.dex)
                        {
                            Console.WriteLine("{0}使用了{1}!", player.name, player.skillList[0].skillname);
                            player.use_skill(player, monster, 0, ref battleinfo);
                            //battleinfo += player.name + "使用了" + player.skillList[0].skillname + "!\n";
                           
                        }
                        else
                        {
                            //battleinfo += monster.name + "闪避成功！\n";
                            Console.WriteLine("{0}使用了{1}!但是{2}闪避成功!", player.name, player.skillList[0].skillname, monster.name);
                        }
                    }
                    //else if (player.hp > 0 && input == 2)
                    //{
                    //    player.useskill(player.player_skills, monster);
                    //}
                    else if (player.hp > 0 && input == 3)
                    {
                        
                            player.use_skill(player,monster,2,ref battleinfo);
                            //battleinfo += player.name + "使用了" + player.skillList[2].skillname + "!\n";
                            Console.WriteLine("{0}使用了{1}!", player.name, player.skillList[2].skillname);
                        
                      
                    }
                    else if (player.hp <= 0)
                    {
                      Console.WriteLine("你死了，游戏结束！");
                       
                    }
                    if (player.hp > 0 && input == 4)
                    {

                        if (devil_slip > monster.dex)
                        {
                            player.use_skill(player, monster, 3, ref battleinfo);
                            //battleinfo += player.name + "成功使用了" + player.skillList[3].skillname + "!\n";
                            Console.WriteLine("{0}使用了{1}!", player.name, player.skillList[3].skillname);
                        }
                        else
                        {
                            //battleinfo += monster.name + "闪避成功！\n";
                            Console.WriteLine("{0}使用了{1}!但是{2}闪避成功!", player.name, player.skillList[3].skillname, monster.name);
                        }
                    }




                }
                //////////////////////////////////////////////////////////////////////////////////
                ///////////////////////////////////////////////////////////////////////////////////、
                ///勇者先攻
                else if (first_attack < 50)
                {
                   // battleinfo += "此回合" + player.name + "先攻！\n";
                    Console.WriteLine("此回合先攻的是{0}！", player.name);
                    if (input == 1)
                    {
                        
                        if (devil_slip > monster.dex)
                        {
                            player.use_skill(player, monster, 0, ref battleinfo);
                            Console.WriteLine("{0}使用了{1}!", player.name, player.skillList[0].skillname);
                        }
                        else
                        {
                            //battleinfo += monster.name + "闪避成功！\n";
                            Console.WriteLine("{0}使用了{1}!但是{2}闪避成功!", player.name, player.skillList[0].skillname, monster.name);
                        }
                    }
                    else if (input == 3)
                    {
                        player.use_skill(player, monster, 2, ref battleinfo);
                        Console.WriteLine("{0}使用了{1}!", player.name, player.skillList[2].skillname);
                    }
                    else if (input == 4)
                    {
                        
                        if (devil_slip > monster.dex)
                        {
                            player.use_skill(player, monster, 3, ref battleinfo);
                            //battleinfo += player.name + "成功使用了" + player.skillList[3].skillname + "!\n";
                            Console.WriteLine("{0}使用了{1}!", player.name, player.skillList[3].skillname);
                        }
                        else
                        {
                            Console.WriteLine("{0}使用了{1}!但是{2}闪避成功!", player.name, player.skillList[3].skillname, monster.name);

                        }             
                    }

                    if (monster.hp > 0)
                    {

                        if (hero_slip > player.dex)
                        {
                            Console.WriteLine("{0}使用了{1}!", monster.name, monster.skillList[0].skillname);
                            monster.use_skill(player, monster, 0, ref battleinfo);
                            
                        }
                        else
                        {
                            Console.WriteLine("{0}使用了{1}!{2}闪避成功!", monster.name, monster.skillList[0].skillname, player.name);
                        }
                    }
                  

                      
                }

            }
            else
            {
                if (player.hp <= 0)
                {
                    battleinfo += "你死了，游戏结束\n";
                 
                }
                else if (monster.hp <= 0)
                {
                    battleinfo+=monster.name+"死了，胜利！\n";
            
                }
            }

        
        }



    }
}
