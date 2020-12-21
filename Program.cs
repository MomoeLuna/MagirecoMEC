using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;

namespace MagirecoMEC
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
        }

    }

    public class NodeTree
    {
        public Node center { get; private set; }
        public int max { get; private set; }
        public int limit { get; private set; }
        public int used { get; private set; }
        public int TotalATK { get; set; }
        public int TotalDEF { get; set; }
        public int TotalHP { get; set; }
        public int TotalA { get; set; }
        public int TotalB { get; set; }
        public int TotalC { get; set; }
        public string Skill { get; set; }
        public List<string> Abilities { get; set; }

        public NodeTree(Node center, int limit, int max=100, int used = 0)
        {
            this.center = center;
            this.max = max;
            this.limit = limit;
            this.used = used;
        }
        public static void AddNode(Node node, Node fornt)
        {
            node.Front = fornt;
            fornt.Next.Add(node);
        }

        public static void DeleteNode(Node node)
        {
            node.Front.Next.Remove(node);
        }

        public void Learn(Node node)
        {
            Node front = node.Front;
            node.Taken = true;
            used++;
            setAttribute(node, SetAttribute.Learn);
            if (used > limit)
            {
                Revert(node);
                Console.WriteLine("Limit Exceeded!");
                return;
            }
            while (front.type!=Type.STR && !front.Taken)
            {
                Learn(front);
            }
        }

        public void Revert(Node node)
        {
            node.Taken = false;
            used--;
            setAttribute(node, SetAttribute.Revert);
            if (node.Next != null)
            {
                List<Node> next = node.Next;
                foreach (Node n in next)
                {
                    Revert(n);
                }
            }
        }

        private void setAttribute(Node node, SetAttribute flag)
        {
            switch (node.type)
            {
                case Type.ATK:
                    this.TotalATK += node.number * (int)flag;
                    break;
                case Type.DEF:
                    this.TotalDEF += node.number * (int)flag;
                    break;
                case Type.HP:
                    this.TotalHP += node.number * (int)flag;
                    break;
                case Type.A:
                    this.TotalA += node.number * (int)flag;
                    break;
                case Type.B:
                    this.TotalB += node.number * (int)flag;
                    break;
                case Type.C:
                    this.TotalC += node.number * (int)flag;
                    break;
                case Type.SKL:
                    this.Skill = node.description;
                    break;
                case Type.ABL:
                    this.Abilities.Add(node.description);
                    break;
                default:
                    return;
            }
        }
    }

    public class Node
    {
        public Type type { get; private set; }
        public string description { get; private set; }
        public int number { get; private set; }
        public List<Node> Next { get; set; }
        public Node Front { get; set; }
        public bool Taken { get; set; }

        public Node(Type type, string description, int number)
        {
            this.type = type;
            this.description = description;
            this.number = number;
            this.Taken = false;
        }
        public void Write()
        {
            Console.Write($"{type.GetDescription()}:{number}");
        }
    }

    public enum Type: byte
    {
        [Description("Center")]
        STR,
        [Description("Skill")]
        SKL,
        [Description("Ability")]
        ABL,
        [Description("Accele")]
        A,
        [Description("Blast")]
        B,
        [Description("Charge")]
        C,
        [Description("ATK")]
        ATK,
        [Description("DEF")]
        DEF,
        [Description("HP")]
        HP
    }

    public enum SetAttribute
    {
        Learn = 1,
        Revert = -1
    }

    public static class Ext
    {
        public static string GetDescription(this Enum enumValue)
        {
            FieldInfo fieldInfo = enumValue.GetType().GetField(enumValue.ToString());
            DescriptionAttribute[] attrs =
                fieldInfo.GetCustomAttributes(typeof(DescriptionAttribute), false) as DescriptionAttribute[];

            return attrs.Length > 0 ? attrs[0].Description : enumValue.ToString();
        }
    }
}
