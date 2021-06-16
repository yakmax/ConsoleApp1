using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    class SearchPoints
    {
        public static List<Point> FindPath(int[,] field, Point start, Point goal)
        {
            // Шаг 1.
            // Создается 2 списка вершин — ожидающие рассмотрения и уже рассмотренные.
            // В ожидающие добавляется точка старта, список рассмотренных пока пуст.
            var closedSet = new Collection<PathNode>();
            var openSet = new Collection<PathNode>();

            // Шаг 2. 
            // Для каждой точки рассчитывается F = G + H.
            // G — расстояние от старта до точки,
            // H — примерное расстояние от точки до цели.
            // О расчете этой величины я расскажу позднее.
            // Так же каждая точка хранит ссылку на точку, из которой в нее пришли.
            
            PathNode startNode = new PathNode()
            {
                Position = start,
                CameFrom = null,
                PathLengthFromStart = 0, //Длина пути от начала
                HeuristicEstimatePathLength = GetHeuristicPathLength(start, goal) 
            };
            openSet.Add(startNode);
            while (openSet.Count > 0)
            {

                // Шаг 3.
                // Из списка точек на рассмотрение выбирается точка с наименьшим F. Обозначим ее X.
                var currentNode = openSet.OrderBy(node =>
                  node.EstimateFullPathLength).First();

                // Шаг 4. 
                // Если X — цель, то мы нашли маршрут.
                if (currentNode.Position == goal)
                    return GetPathForNode(currentNode);

                // Шаг 5.
                // Переносим X из списка ожидающих рассмотрения в список уже рассмотренных.
                openSet.Remove(currentNode);
                //Console.WriteLine(openSet.Count);
                closedSet.Add(currentNode);

                // Шаг 6.
                // Для каждой из точек, соседних для X (обозначим эту соседнюю точку Y), делаем следующее:
                foreach (var neighbourNode in GetNeighbours(currentNode, goal, field))
                {
                    // Шаг 7.
                    // Если Y уже находится в рассмотренных — пропускаем ее.
                    if (closedSet.Count(node => node.Position == neighbourNode.Position) > 0)
                        continue;
                    var openNode = openSet.FirstOrDefault(node =>
                      node.Position == neighbourNode.Position);

                    // Шаг 8.
                    // Если Y еще нет в списке на ожидание —
                    // добавляем ее туда, запомнив ссылку на X и рассчитав Y.G (это X.G + расстояние от X до Y) и Y.H.
                    if (openNode == null)
                        openSet.Add(neighbourNode);
                    else
                      if (openNode.PathLengthFromStart > neighbourNode.PathLengthFromStart)
                    {
                        // Шаг 9.
                        // Если же Y в списке на рассмотрение —
                        // проверяем, если X.G + расстояние от X до Y < Y.G, значит мы пришли в точку Y более коротким путем,
                        // заменяем Y.G на X.G + расстояние от X до Y, а точку, из которой пришли в Y на X.
                        openNode.CameFrom = currentNode;
                        openNode.PathLengthFromStart = neighbourNode.PathLengthFromStart;
                    }
                }
            }
            // Шаг 10.
            // Если список точек на рассмотрение пуст, а до цели мы так и не дошли — значит маршрут не существует.
            //Console.WriteLine("Пуст");
            return null;
        }

        //Первая из них — функция расстояния от X до Y:
        //Расстояние между соседними клетками всегда равно 1.
        private static int GetDistanceBetweenNeighbours()
        {
            return 1;
        }

        // Функция примерной оценки расстояния до цели:
        private static int GetHeuristicPathLength(Point from, Point to)
        {
            return Math.Abs(from.X - to.X) + Math.Abs(from.Y - to.Y);
        }

        private static Collection<PathNode> GetNeighbours(PathNode pathNode,
  Point goal, int[,] field)
        {
            var result = new Collection<PathNode>();

            // Соседними точками являются соседние по стороне клетки.
            Point[] neighbourPoints = new Point[4];
            neighbourPoints[0] = new Point(pathNode.Position.X + 1, pathNode.Position.Y);
            neighbourPoints[1] = new Point(pathNode.Position.X - 1, pathNode.Position.Y);
            neighbourPoints[2] = new Point(pathNode.Position.X, pathNode.Position.Y + 1);
            neighbourPoints[3] = new Point(pathNode.Position.X, pathNode.Position.Y - 1);

            foreach (var point in neighbourPoints)
            {
                // Проверяем, что не вышли за границы карты.
                if (point.X < 0 || point.X >= field.GetLength(0))
                    continue;
                if (point.Y < 0 || point.Y >= field.GetLength(1))
                    continue;
                // Проверяем, что по клетке можно ходить.
                if ((field[point.X, point.Y] != 0) && (field[point.X, point.Y] != 1))
                    continue;
                // Заполняем данные для точки маршрута.
                var neighbourNode = new PathNode()
                {
                    Position = point,
                    CameFrom = pathNode,
                    PathLengthFromStart = pathNode.PathLengthFromStart +
                    GetDistanceBetweenNeighbours(),
                    HeuristicEstimatePathLength = GetHeuristicPathLength(point, goal)
                };
                result.Add(neighbourNode);
            }
            return result;
        }

        // Получения маршрута. Маршрут представлен в виде списка координат точек.
        private static List<Point> GetPathForNode(PathNode pathNode)
        {
            var result = new List<Point>();
            var currentNode = pathNode;
            while (currentNode != null)
            {
                result.Add(currentNode.Position);
                currentNode = currentNode.CameFrom;
            }
            result.Reverse();
            foreach (Point i in result)
            {
                
                Console.Write(i.X);
                Console.Write(" ");
                Console.Write(i.Y);
                Console.WriteLine("");
            }
            return result;
        }

    }
}
