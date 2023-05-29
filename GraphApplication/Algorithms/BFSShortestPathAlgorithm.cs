﻿using GraphApplication.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace GraphApplication.Algorithms
{
    public class BFSShortestPathAlgorithm : IIterativeAlgorithm
    {
        private IEnumerable<VertexModel> BuildPathByParents(
            ref Dictionary<VertexModel, VertexModel> parents,
            VertexModel end)
        {
            List<VertexModel> path = new List<VertexModel>();

            while (true)
            {
                path.Insert(0, end);

                if (parents.ContainsKey(end) == false)
                {
                    break;
                }

                end = parents[end];
            }

            return path;
        }

        public IEnumerable<VertexModel>? Implement(GraphModel graph, params object[] args)
        {
            if (args.Length < 2)
                throw new ArgumentException("Для знаходження шляху потрібно передати 2 аргументи!"); 



            if (args[0] is VertexModel start && args[1] is VertexModel end)
            {
                Dictionary<VertexModel, VertexModel> parents = new();
             //   Dictionary<VertexModel, int> distances = new();
                HashSet<VertexModel> visited = new();

                Queue<VertexModel> queue = new Queue<VertexModel>();

                queue.Enqueue(start);
              //  distances[start] = 0;


                while (queue.Count > 0)
                {
                    VertexModel topElement = queue.Dequeue();

                    foreach (VertexModel neighbor in graph.AdjancencyDictionary[topElement])
                    {
                        if (visited.Contains(neighbor) == false && neighbor.IsActive)
                        {
                            queue.Enqueue(neighbor);
                            visited.Add(topElement);

                            if(parents.ContainsKey(neighbor) == false)
                                parents[neighbor] = topElement;

                            if (neighbor == end)
                                return BuildPathByParents(ref parents, end);

                        }
                    }
                }

                return null;
            }
            else
            {
                throw new ArgumentException("Для знаходження шляху потрібно передати 2 вершини!");
            }
        }
    }
}
