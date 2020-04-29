using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
namespace rpg
{  
    class Role
    {
        public string name
        {
            get;
            protected set;
        }
        public int hp
        {
            get;
            set;
        }
        public int mp
        {
            get;
            set;
        }
        public int base_attackValue
        {       
            get;
             set;

            }
        public int defense
        {
            get;
             set;
        }
        public int dex
        {
            get;
            set;//闪避几率
        }

        public Dictionary<int, Skill> skillList = new Dictionary<int, Skill>();
        //public List<state> states = new List<state>();
        public List<item> itemList = new List<item>();
        public Role(string name, int hp, int mp,int base_attackValue, int defense, int dex, int x,int y)
        {
            this.name = name;
            this.hp = hp;
            this.mp = mp;
            this.base_attackValue = base_attackValue;
            this.defense = defense;
            this.dex = dex;
            pos.x = x;
            pos.y = y;
           
        }

        public Pos pos=new Pos(0,0);
        public virtual void use_skill(Player player, Monster monster, int skillType,ref string battleinfo)
        {
            if (player.skillList.ContainsKey(skillType))
            {
                if (skillType == 0)
                {
                    monster.hp = monster.hp + monster.defense - player.skillList[0].damage;

                }
                else if (skillType == 2)
                {
                    player.hp = player.hp + player.skillList[2].damage;  
                }
                else if (skillType == 3)
                {
                    if (monster.hp < 200)
                    {
                        monster.hp = 0;
                    }
                    else
                    {
                        battleinfo += "敌方生命值不可能斩杀\n";
                    }
                }
                //else if (skillType == 1)
                //{   new
                //    states.Add()
                //}
            }
        }

    



    }
    class Skill
    { public string skillname;
        public int skillTpye;
        public int damage;
        public int skill_overtime;
        public int execute_dangmage;
        public int mpcost;
        public Skill(string name,int skillTpye, int damage, int skill_overtime, int execute_dangmage,int mpcost)
        {
            this.skillname = name;
            this.skillTpye = skillTpye;
            this.damage = damage;
            this.skill_overtime = skill_overtime;
            this.execute_dangmage = execute_dangmage;
            this.mpcost = mpcost;
         }
      
    }
    //class state
    //{
    //   public int skill_time;

    //}
    class item
    {
        public string name;
        public int add_Hp;
        public  int add_baseattack;
        public int add_cure_time;
        public int add_defense;
        public int add_dex;
        //public int add_mp;
        public int cost;
        public item(string name, int add_Hp,int add_baseattack, int add_cure_time, int add_defense, int add_dex,int cost)
        {
            this.name = name;
            this.add_Hp = add_Hp;
            this.add_baseattack = add_baseattack;
            this.add_cure_time = add_cure_time;
            this.add_defense = add_defense;
            this.add_dex = add_dex;
            this.cost = cost;
        }



    }
    class special_item : item
    {
        public string[,] skilleffectArea;
        public special_item(string name, int add_Hp,int add_baseattack, int add_cure_time, int add_defense, int add_dex,int cost) : base(name,add_Hp, add_baseattack, add_cure_time, add_defense, add_dex,cost)
        {
          skilleffectArea=new string[1,5];
           
        }
    
    }

     class NPC
     {  
        public  List<item> itemlist = new List<item>();
        public string name;
        public int id;
        public Pos pos=new Pos(logicControl.Width-3,logicControl.Height-30);
        public NPC(string _name, int _id,int x,int y)
        {
            name = _name;
            id = _id;
            pos.x = x;
            pos.y = y;
        }

        public void SetPos(int x, int y)
        {
            pos.x= x;
            pos.y = y;
        }

        public virtual void OnTalk(Player player, out string text)
        {    
            text = "这里是造换塔控制台！请输入购买的商品：1高浓缩口粮2高频共振刀3皮肤硬化剂4肾上腺激素";
            salelist();
        }

       
        public void salelist()
        {
            item item1 = new item("高浓缩口粮", 200, 0, 0, 0, 0, 50);
            item item2 = new item("高频共振刀", 0, 100, 0, 0, 0, 300);
            item item3 = new item("皮肤硬化剂", 0, 0, 0, 50, 0, 300);
            item item4 = new item("肾上腺激素", 0, 0, 0, 0, 20, 300);
            itemlist.Add(item1);
            itemlist.Add(item2);
            itemlist.Add(item3);
            itemlist.Add(item4);
        }

         public object AfterDisappear()
        {
            return null;
        }
    }
    class Gate : NPC
    {
        public Gate(string name,int id,int x,int y) : base(name, id, x, y)
        {
            this.name = name;
            this.id = id;
            pos.x = x;
            pos.y = y;
        }
        public override void OnTalk(Player player, out string text)
        {
            text = "这里是构造传送塔，必须使全区域安全后才能传送";
        }


    }

}
