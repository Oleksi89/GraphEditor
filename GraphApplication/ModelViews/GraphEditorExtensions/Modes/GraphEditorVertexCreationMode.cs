﻿using GraphApplication.Models;
using System.Diagnostics;
using System.Windows;
using System.Windows.Input;

namespace GraphApplication.ModelViews.GraphEditorExtensions.Modes
{
    class GraphEditorVertexCreationMode : GraphEditorMode, IBuildingMode
    {
        public GraphEditorVertexCreationMode(GraphEditorModelView modelView) : base(modelView)
        {
            _modelView.SelectionManager.DiselectAll();
        }

        public override void EditorMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (sender is IInputElement)
            {
                _modelView.IsSaved = false;

                Point createPoint = Mouse.GetPosition((IInputElement)sender);

                //gap position into canvas
                createPoint.X -= _modelView.OffsetX;
                createPoint.Y -= _modelView.OffsetY;
                createPoint.X /= _modelView.ScaleValue;
                createPoint.Y /= _modelView.ScaleValue;

                VertexModelView vertexModelView = new VertexModelView(
                    new VertexModel(createPoint.X, createPoint.Y, _modelView.GraphModelView.GetFreeIndex()));

                _modelView.GraphModelView.VertexModelViews.Add(vertexModelView);

                Trace.WriteLine("Vertex created at point: " + createPoint.X + " " + createPoint.Y);
            }
        }

        public override void OnModeSwitch()
        {
        }
    }
}