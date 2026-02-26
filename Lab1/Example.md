```csharp
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
namespace SimpleLINQ
{
  class Program
  {
    /// <summary>
    /// Клас данних
    /// </summary>
    public class Data
    {
      /// <summary>
      /// Ключ
      /// </summary>
      public int id;
      /// <summary>
      /// Для групування
      /// </summary>
      public string grp;
      /// <summary>
      /// Значення
      /// </summary>
      public string value;
      /// <summary>
      /// Конструктор
      /// </summary>
      public Data(int i, string g, string v)
      {
        this.id = i;
        this.grp = g;
        this.value = v;
      }
      /// <summary>
      /// Приведення до строкового типу
      /// </summary>
      public override string ToString()
      {
        return string.Format($"(id={this.id}; grp={this.grp}; value={this.value})");
      }
    }
    /// <summary>
    /// Клас для порівняння даних
    /// </summary>
    public class DataEqualityComparer : IEqualityComparer<Data>
    {
      public bool Equals(Data x, Data y)
      {
        bool Result = false;
        if (x.id == y.id && x.grp == y.grp && x.value == y.value)
          Result = true;
        return Result;
      }
      public int GetHashCode(Data obj)
      {
        return obj.id;
      }
    }
    /// <summary>
    /// Зв'язок між двома колекціями
    /// </summary>
    public class DataLink
    {
      public int d1;
      public int d2;
      public DataLink(int i1, int i2)
      {
        this.d1 = i1;
        this.d2 = i2;
      }
    }
    //Приклад даних
    static List<Data> d1 = new List<Data>()
{
new Data(1, "group1", "11"),
new Data(2, "group1", "12"),
new Data(3, "group2", "13"),
new Data(5, "group2", "15")
};
    static List<Data> d2 = new List<Data>()
{
new Data(1, "group2", "21"),
new Data(2, "group3", "221"),
new Data(2, "group3", "222"),
new Data(4, "group3", "24")
};
    static List<Data> d1_for_distinct = new List<Data>()
{
new Data(1, "group1", "11"),
new Data(1, "group1", "11"),
new Data(1, "group1", "11"),
new Data(2, "group1", "12"),
new Data(2, "group1", "12")
};
    static List<DataLink> lnk = new List<DataLink>()
{
new DataLink(1, 1),
new DataLink(1, 2),
new DataLink(1, 4),
new DataLink(2, 1),
new DataLink(2, 2),
new DataLink(2, 4),
new DataLink(5, 1),
new DataLink(5, 2)
};
    static void Main(string[] args)
    {
      Console.OutputEncoding = Encoding.UTF8;
      Console.ForegroundColor = ConsoleColor.Green;
      Console.WriteLine("Прості запити");
      Console.ResetColor();
      Console.WriteLine("Проста вибірка елементів");
      var q1 = from x in d1
               select x;
      foreach (var x in q1)
        Console.WriteLine(x);
      Console.WriteLine("Вибірка окремого поля (проекція)");
      var q2 = from x in d1
               select x.value;
      foreach (var x in q2)
        Console.WriteLine(x);
      Console.WriteLine("Створення нового об'єкту анонімного типу");
      var q3 = from x in d1
               select new { identifier = x.id, value = x.value };
      foreach (var x in q3)
        Console.WriteLine(x);
      Console.WriteLine("Умови");
      var q4 = from x in d1
               where x.id > 1 && (x.grp == "group1" || x.grp == "group2")
               select x;
      foreach (var x in q4)
        Console.WriteLine(x);
      Console.WriteLine("Вибірка значення конкретного типу");
      object[] array = new object[] { 123, "рядок 1", true, "рядок 2" };
      var qo = from x in array.OfType<string>()
               select x;
      foreach (var x in qo)
        Console.WriteLine(x);
      Console.WriteLine("Сортування");
      var q5 = from x in d1
               where x.id > 1 && (x.grp == "group1" || x.grp == "group2")
               orderby x.grp descending, x.id descending
               select x;
      foreach (var x in q5)
        Console.WriteLine(x);
      Console.WriteLine("Сортування з використанням розширюючих методів");
      var q51 = d1.Where((x) =>
      {
        return x.id > 1 && (x.grp == "group1" || x.grp == "group2");
      }).OrderByDescending(x => x.grp).ThenByDescending(x => x.id);
      foreach (var x in q51) Console.WriteLine(x);
      Console.ForegroundColor = ConsoleColor.Green;
      Console.WriteLine("Partitioning Operators");
      Console.ResetColor();
      Console.WriteLine("Посторінковий вивід даних");
      var qp = GetPage(d1, 2, 2);
      foreach (var x in qp)
        Console.WriteLine(x);
      Console.WriteLine("Використання SkipWhile и TakeWhile");
      int[] intArray = new int[] { 1, 2, 3, 4, 5, 6, 7, 8 };
      var qw = intArray.SkipWhile(x => (x < 4)).TakeWhile(x => x <= 7);
      foreach (var x in qw)
        Console.WriteLine(x);
      Console.ForegroundColor = ConsoleColor.Green;
      Console.WriteLine("З'єднання");
      Console.ResetColor();
      Console.WriteLine("Декартовий добуток");
      var q6 = from x in d1
               from y in d2
               select new { v1 = x.value, v2 = y.value };
      foreach (var x in q6)
        Console.WriteLine(x);
      Console.WriteLine("Inner Join з використанням Where");
      var q7 = from x in d1
               from y in d2
               where x.id == y.id
               select new { v1 = x.value, v2 = y.value };
      foreach (var x in q7)
        Console.WriteLine(x);
      Console.WriteLine("Inner Join з використанням Join");
      var q8 = from x in d1
               join y in d2 on x.id equals y.id
               select new { v1 = x.value, v2 = y.value };
      foreach (var x in q8)
        Console.WriteLine(x);
      Console.WriteLine("Inner Join (зберігаємо об'єкт)");
      var q9 = from x in d1
               join y in d2 on x.id equals y.id
               select new { v1 = x.value, d2Group = y };
      foreach (var x in q9)
        Console.WriteLine(x);
      //Обираємо всі елементи з d1 та якщо є пов'язані з d2 (outer join)
      //В temp поміщуємо всю групу, далі її елементи перебираємо окремо
      Console.WriteLine("Group Join");
      var q10 = from x in d1
                join y in d2 on x.id equals y.id into temp
                select new { v1 = x.value, d2Group = temp };
      foreach (var x in q10)
      {
        Console.WriteLine(x.v1);
        foreach (var y in x.d2Group)
          Console.WriteLine(" " + y);
      }
      Console.WriteLine("Cross Join и Group Join");
      var q11 = from x in d1
                join y in d2 on x.id equals y.id into temp
                from t in temp
                select new { v1 = x.value, v2 = t.value, cnt = temp.Count() };
      foreach (var x in q11)
        Console.WriteLine(x);
      Console.WriteLine("Outer Join");
      var q12 = from x in d1
                join y in d2 on x.id equals y.id into temp
                from t in temp.DefaultIfEmpty()
                select new { v1 = x.value, v2 = ((t == null) ? "null" : t.value) };
      foreach (var x in q12)
        Console.WriteLine(x);
      Console.WriteLine("Використання Join для складених ключів");
      var q12_1 = from x in d1
                  join y in d1_for_distinct on new { x.id, x.grp } equals new
                  {
                    y.id,
                    y.grp
                  } into details
                  from d in details
                  select d;
      foreach (var x in q12_1)
        Console.WriteLine(x);
      //Дії над множинами
      Console.ForegroundColor = ConsoleColor.Green;
      Console.WriteLine("Дії над множинами");
      Console.ResetColor();
      Console.WriteLine("Distinct - для значень");
      var q13 = (from x in d1 select x.grp).Distinct();
      foreach (var x in q13)
        Console.WriteLine(x);
      Console.WriteLine("Distinct - для об'єктів");
      var q15 = (from x in d1_for_distinct select x).Distinct(new
      DataEqualityComparer());
      foreach (var x in q15)
        Console.WriteLine(x);
      Console.WriteLine("Union - об'єднання з виключенням дублікатів");
      int[] i1 = new int[] { 1, 2, 3, 4 };
      int[] i1_1 = new int[] { 2, 3, 4, 1 };
      int[] i2 = new int[] { 2, 3, 4, 5 };
      foreach (var x in i1.Union(i2))
        Console.WriteLine(x);
      Console.WriteLine("Union - об'єднання для объектів");
      foreach (var x in d1.Union(d1_for_distinct))
        Console.WriteLine(x);
      Console.WriteLine("Union - об'єднання для объектів з виключенням дублікатів 1");
      foreach (var x in d1.Union(d1_for_distinct, new DataEqualityComparer()))
        Console.WriteLine(x);
      Console.WriteLine("Union - об'єднання для объектів з виключенням дублікатів 2");
      foreach (var x in d1.Union(d1_for_distinct).Union(d2).Distinct(new
      DataEqualityComparer()))
        Console.WriteLine(x);
      Console.WriteLine("Concat - об'єднання без виключення дублікатів");
      foreach (var x in i1.Concat(i2))
        Console.WriteLine(x);
      Console.WriteLine("SequenceEqual - перевірка співпадіння та порядку слідування");
      Console.WriteLine(i1.SequenceEqual(i1));
      Console.WriteLine(i1.SequenceEqual(i2));
      Console.WriteLine("Intersect - перетин множин");
      foreach (var x in i1.Intersect(i2))
        Console.WriteLine(x);
      Console.WriteLine("Intersect - перетин множин для об'єктів");
      foreach (var x in d1.Intersect(d1_for_distinct, new DataEqualityComparer()))
        Console.WriteLine(x);
      Console.WriteLine("Except - різниця множин");
      foreach (var x in i1.Except(i2))
        Console.WriteLine(x);
      Console.WriteLine("Except - різниця множин для об'єктів");
      foreach (var x in d1.Except(d1_for_distinct, new DataEqualityComparer()))
        Console.WriteLine(x);
      Console.ForegroundColor = ConsoleColor.Green;
      Console.WriteLine("Функції агрегування");
      Console.ResetColor();
      Console.WriteLine("Count - кількість елементів");
      Console.WriteLine(d1.Count());
      Console.WriteLine("Count - кількість з умовою");
      Console.WriteLine(d1.Count(x => x.id > 1));
      Console.WriteLine("Aggregate - агрегування значень");
      var qa1 = d1.Aggregate(new Data(0, "", ""), (total, next) =>
      {
        if (next.id > 1) total.id += next.id;
        return total;
      });
      Console.WriteLine(qa1);
      Console.WriteLine("Групування");
      var q16 = from x in d1.Union(d2)
                group x by x.grp into g
                select new
                {
                  Key =
      g.Key,
                  Values = g
                };
      foreach (var x in q16)
      {
        Console.WriteLine(x.Key);
        foreach (var y in x.Values)
          Console.WriteLine(" " + y);
      }
      Console.WriteLine("Групування з функціями агрегування");
      var q17 = from x in d1.Union(d2)
                group x by x.grp into g
                select new
                {
                  Key = g.Key,
                  Values = g,
                  cnt = g.Count(),
                  cnt1 =
                g.Count(x => x.id > 1),
                  sum = g.Sum(x => x.id),
                  min = g.Min(x => x.id)
                };
      foreach (var x in q17)
      {
        Console.WriteLine(x);
        foreach (var y in x.Values)
          Console.WriteLine(" " + y);
      }
      Console.WriteLine("Групування - Any");
      var q18 = from x in d1.Union(d2)
                group x by x.grp into g
                where g.Any(x => x.id > 3)
                select new { Key = g.Key, Values = g };
      foreach (var x in q18)
      {
        Console.WriteLine(x.Key);
        foreach (var y in x.Values)
          Console.WriteLine(" " + y);
      }
      Console.WriteLine("Групування - All");
      var q19 = from x in d1.Union(d2)
                group x by x.grp into g
                where g.All(x => x.id > 1)
                select new { Key = g.Key, Values = g };
      foreach (var x in q19)
      {
        Console.WriteLine(x.Key);
        foreach (var y in x.Values)
          Console.WriteLine(" " + y);
      }
      Console.ForegroundColor = ConsoleColor.Green;
      Console.WriteLine("Перетворення даних");
      Console.ResetColor();
      Console.WriteLine("Результат перетворюється в масив");
      var e3 = (from x in d1 select x).ToArray();
      Console.WriteLine(e3.GetType().Name);
      foreach (var x in e3)
        Console.WriteLine(x);
      Console.WriteLine("Результат перетворюється в Dictionary");
      var e4 = (from x in d1 select x).ToDictionary(x => x.id);
      Console.WriteLine(e4.GetType().Name);
      foreach (var x in e4)
        Console.WriteLine(x);
      Console.WriteLine("Результат перетворюється в Lookup");
      var e5 = (from x in d1_for_distinct select x).ToLookup(x => x.id);
      Console.WriteLine(e5.GetType().Name);
      foreach (var x in e5)
      {
        Console.WriteLine(x.Key);
        foreach (var y in x)
          Console.WriteLine(" " + y);
      }
      Console.ForegroundColor = ConsoleColor.Green;
      Console.WriteLine("Отримання елемента");
      Console.ResetColor();
      Console.WriteLine("Перший елемент з вибірки");
      var f1 = (from x in d2 select x).First(x => x.id == 2);
      Console.WriteLine(f1);
      Console.WriteLine("Перший елемент з вибірки або значення за замовчанням");
      var f2 = (from x in d2 select x).FirstOrDefault(x => x.id == 22);
      Console.WriteLine(f2 == null ? "null" : f2.ToString());
      Console.WriteLine("Елемент в заданій позиції");
      var f3 = (from x in d2 select x).ElementAt(2);
      Console.WriteLine(f3);
      Console.ForegroundColor = ConsoleColor.Green;
      Console.WriteLine("Генерація послідовностей");
      Console.ResetColor();
      Console.WriteLine("Range");
      foreach (var x in Enumerable.Range(1, 5))
        Console.WriteLine(x);
      Console.WriteLine("Repeat");
      foreach (var x in Enumerable.Repeat<int>(10, 3))
        Console.WriteLine(x);
      Console.ReadLine();
    }
    /// <summary>
    /// Отримання певної сторінки даних
    /// </summary>
    static List<Data> GetPage(List<Data> data, int pageNum, int pageSize)
    {
      //Кількість елементів котрі пропускаємо
      int skipSize = (pageNum - 1) * pageSize;
      var q = data.OrderBy(x => x.id).Skip(skipSize).Take(pageSize);
      return q.ToList();
    }
  }
}
```
