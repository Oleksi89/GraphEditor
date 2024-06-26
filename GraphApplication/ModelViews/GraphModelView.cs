﻿using GraphApplication.Commands;
using GraphApplication.Exceptions;
using GraphApplication.Models;
using GraphApplication.Models.Graph;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace GraphApplication.ModelViews
{
    /// <summary>
    /// Manages binding between graph model and view.
    /// </summary>
    public class GraphModelView : NotifyModelView
    {
        public int VertexCount { get => Model.GetVerticesCount(); }
        public int EdgeCount { get => Model.GetEdgesCount(); }

        #region Commands

        // here will be commands such remove vertex, edge, add vertex, edge, they will change model, and after model will change modelView

        private RelayCommand? _removeVertexCommand;

        public RelayCommand RemoveVertexCommand => _removeVertexCommand ??= new RelayCommand((parameter) =>
        {
            var vertexModel = (VertexModel?)parameter;

            if (vertexModel == null)
                throw new ArgumentException(nameof(vertexModel));

            Model.RemoveVertex(vertexModel);
        });

        private RelayCommand? _addVertexCommand;

        // this is demo-version of command, this is a question who should build the actual model, view, or modelView?
        public RelayCommand AddVertexCommand => _addVertexCommand ??= new RelayCommand((parameter) =>
        {
            var vertex = (VertexModel?)parameter;

            if (vertex == null)
                throw new ArgumentException(nameof(vertex));

            Model.AddVertex(vertex);
        });

        private RelayCommand? _addEdgeCommand;

        public RelayCommand AddEdgeCommand => _addEdgeCommand ??= new RelayCommand(parameter =>
        {
            var edge = (EdgeModel?)parameter;

            if (edge == null)
                throw new ArgumentException(nameof(edge));

            Model.AddEdge(edge);
        });

        private RelayCommand? _removeEdgeCommand;

        public RelayCommand RemoveEdgeCommand => _removeEdgeCommand ??= new RelayCommand(parameter =>
        {
            var edge = (EdgeModel?)parameter;

            if (edge == null)
                throw new ArgumentException(nameof(edge));

            Model.RemoveEdge(edge);
        });


        #endregion

        public IGraphModel Model { get; private set; }

        public GraphModelView(IGraphModel model)
        {
            Model = model;
            VertexModelViews = new();
            EdgeModelViews = new();
            _edgeBinding = new();
            _vertexBinding = new();

            AssignEvents();
            AssignPropertyDependency();
            CreateModelViews();
        }

        private void AssignEvents()
        {
            // this private methods depends from graph model, when model changes, view model updates, then it notificate the view.

            Model.OnEdgeAdded += (sender, edge) => AddEdgeModelView(edge);
            Model.OnEdgeRemoved += (sender, edge) => RemoveEdgeModelView(edge);
            Model.OnVertexAdded += (sender, vertex) => AddVertexModelView(vertex);
            Model.OnVertexRemoved += (sender, vertex) => RemoveVertexModelView(vertex);
        }

        private void AssignPropertyDependency()
        {
            Model.OnEdgeAdded += (sender, edge) => OnPropertyChanged(nameof(EdgeCount));
            Model.OnEdgeRemoved += (sender, edge) => OnPropertyChanged(nameof(EdgeCount));
            Model.OnVertexAdded += (sender, vertex) => OnPropertyChanged(nameof(VertexCount));
            Model.OnVertexRemoved += (sender, vertex) => OnPropertyChanged(nameof(VertexCount));

        }

        private void AddEdgeModelView(EdgeModel edge)
        {
            var start = GetVertexModelView_By_VertexModel(edge.Start);
            var end = GetVertexModelView_By_VertexModel(edge.End);

            var edgeModelView = new EdgeModelView((WeightedEdgeModel<double>)edge, start, end);
            // bind
            _edgeBinding[edge] = edgeModelView;

            EdgeModelViews.Add(edgeModelView);
        }

        private void RemoveEdgeModelView(EdgeModel edge)
        {
            var edgeModelView = GetEdgeModelView_By_EdgeModel(edge);

            // unbind
            _edgeBinding.Remove(edge);

            EdgeModelViews.Remove(edgeModelView);
        }

        private void AddVertexModelView(VertexModel vertex)
        {
            var vertexModelView = new VertexModelView(vertex);

            // bind
            _vertexBinding[vertex] = vertexModelView;

            VertexModelViews.Add(vertexModelView);
        }

        private void RemoveVertexModelView(VertexModel vertex)
        {
            var vertexModelView = GetVertexModelView_By_VertexModel(vertex);

            // unbind
            _vertexBinding.Remove(vertex);

            VertexModelViews.Remove(vertexModelView);
        }

        private void CreateModelViews()
        {
            foreach (var vertex in Model.GetVertices())
                AddVertexModelView(vertex);

            foreach (var edge in Model.GetEdges())
                AddEdgeModelView(edge);
        }

        public ObservableCollection<VertexModelView> VertexModelViews { get; private set; }
        public ObservableCollection<EdgeModelView> EdgeModelViews { get; private set; }

        private Dictionary<EdgeModel, EdgeModelView> _edgeBinding;
        private Dictionary<VertexModel, VertexModelView> _vertexBinding;

        public EdgeModelView GetEdgeModelView_By_EdgeModel(EdgeModel edgeModel)
        {
            if (_edgeBinding.ContainsKey(edgeModel))
                return _edgeBinding[edgeModel];

            throw new UnbindedModelViewException("EdgeModel don't have edgeModelView");
        }

        public VertexModelView GetVertexModelView_By_VertexModel(VertexModel vertexModel)
        {
            if (_vertexBinding.ContainsKey(vertexModel))
                return _vertexBinding[vertexModel];

            throw new UnbindedModelViewException("VertexModel don't have vertexModelView");
        }

        public List<EdgeModelView> GetEdgeModelViews_By_EdgeModels(IEnumerable<EdgeModel> models)
        {
            List<EdgeModelView> edgeModelViews = new();

            foreach (var model in models)
            {
                edgeModelViews.Add(GetEdgeModelView_By_EdgeModel(model));
            }

            return edgeModelViews;
        }

        public List<VertexModelView> GetVertexModelViews_By_VertexModels(IEnumerable<VertexModel> models)
        {
            List<VertexModelView> vertexModelViews = new();

            foreach (var model in models)
            {
                vertexModelViews.Add(GetVertexModelView_By_VertexModel(model));
            }

            return vertexModelViews;
        }
    }
}
