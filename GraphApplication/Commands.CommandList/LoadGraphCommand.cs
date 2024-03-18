﻿using GraphApplication.Models;
using GraphApplication.ModelViews;
using GraphApplication.Services;
using GraphApplication.Views;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace GraphApplication.Commands.CommandList
{
    public class LoadGraphCommand: Command
    {
        private MainWindowModelView _mainWindowsViewModel;
        private IFileService<GraphEditorModel> _fileService;

        public LoadGraphCommand(MainWindowModelView mainWindowModelView) : this(mainWindowModelView, new XMLFileService<GraphEditorModel>()) { }

        public LoadGraphCommand(MainWindowModelView mainWindowModelView, IFileService<GraphEditorModel> fileService)
        {
            _mainWindowsViewModel = mainWindowModelView;
            _fileService = fileService;
        }

        public override void Execute(object? parameter)
        {
            //promt load location
            var openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "XML files (*.xml)|*.xml";

            if (openFileDialog.ShowDialog() == true)
            {
                string path = openFileDialog.FileName;
                if (_mainWindowsViewModel.OpenedGraphModelViewsManager.IsOpened(path))
                {
                    MessageBox.Show("Такий граф вже відкритий!", "Попередження", MessageBoxButton.OK, MessageBoxImage.Information);
                    return;
                }

                string name = System.IO.Path.GetFileName(path);

                GraphEditorModel modelView = _fileService.Open(path);

                if (modelView == null)
                    throw new Exception();

                GraphEditorModelView graphEditorModelView = new GraphEditorModelView(modelView, name, true);
                _mainWindowsViewModel.OpenedGraphModelViewsManager.AssignModelView(graphEditorModelView, path);
                _mainWindowsViewModel.OpenedGraphModelViewsManager.GraphEditorViews.Add(graphEditorModelView);
                _mainWindowsViewModel.OpenedGraphModelViewsManager.SelectedView = graphEditorModelView;
            }
        }
    }
}
