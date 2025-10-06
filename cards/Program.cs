using System;
using System.Collections.Generic;

namespace cards
{
    // Интерфейс
    public interface IPlayable
    {
        void Play();
    }

    // Абстрактный класс карты
    public abstract class Card : IPlayable
    {
        public string Name { get; set; }
        public int ManaCost { get; set; }

        public Card(string name, int manaCost)
        {
            Name = name;
            ManaCost = manaCost;
        }

        public abstract void Play();

        public override string ToString()
        {
            return $"{Name} (стоимость маны: {ManaCost})";
        }
    }

    // Класс карты-существа
    public class CreatureCard : Card
    {
        public int Attack { get; set; }
        public int Health { get; set; }

        public CreatureCard(string name, int manaCost, int attack, int health)
            : base(name, manaCost)
        {
            Attack = attack;
            Health = health;
        }

        public override void Play()
        {
            Console.WriteLine($"{Name} выходит на поле боя! ({Attack}/{Health})");
        }
    }

    // Класс карты-заклинания
    public class SpellCard : Card
    {
        public string Effect { get; set; }

        public SpellCard(string name, int manaCost, string effect)
            : base(name, manaCost)
        {
            Effect = effect;
        }

        public override void Play()
        {
            Console.WriteLine($"Применено заклинание '{Name}'! Эффект: {Effect}");
        }
    }

    // Класс карты-предмета
    public class ItemCard : Card
    {
        public string Bonus { get; set; }

        public ItemCard(string name, int manaCost, string bonus)
            : base(name, manaCost)
        {
            Bonus = bonus;
        }

        public override void Play()
        {
            Console.WriteLine($"Использован предмет '{Name}'! Бонус: {Bonus}");
        }
    }

    // Исключение при пустой колоде
    public class EmptyDeckException : Exception
    {
        public EmptyDeckException() : base("Ошибка: колода пуста!") { }
    }

    // Класс игры
    public class Game
    {
        private Queue<Card> deck = new Queue<Card>();
        private Stack<Card> discardPile = new Stack<Card>();

        public Game()
        {
            // Заполняем колоду
            deck.Enqueue(new CreatureCard("Дракон", 5, 8, 8));
            deck.Enqueue(new SpellCard("Огненный шар", 3, "наносит 5 урона"));
            deck.Enqueue(new ItemCard("Меч героя", 2, "+3 к атаке"));
            deck.Enqueue(new CreatureCard("Эльф", 2, 2, 3));
        }

        public void Start()
        {
            Console.WriteLine("=== Симулятор карточной игры ===");
            Console.WriteLine("Нажмите Enter, чтобы сыграть карту. 'q' — выход.");

            while (true)
            {
                Console.Write("> ");
                string input = Console.ReadLine();
                if (input == "q") break;

                try
                {
                    PlayNextCard();
                }
                catch (EmptyDeckException e)
                {
                    Console.WriteLine(e.Message);
                    break;
                }
            }

            Console.WriteLine("\nКарты в сбросе:");
            foreach (var card in discardPile)
            {
                Console.WriteLine("- " + card);
            }
        }

        private void PlayNextCard()
        {
            if (deck.Count == 0)
                throw new EmptyDeckException();

            var card = deck.Dequeue();
            card.Play();
            discardPile.Push(card);
        }
    }

    // Точка входа
    class Program
    {
        static void Main()
        {
            var game = new Game();
            game.Start();
        }
    }
}

