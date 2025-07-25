﻿using DynamicData;
using LinqSTG.Demo.NodeGraph.ViewModel.Editor;
using NodeNetwork.Toolkit.ValueNode;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace LinqSTG.Demo.NodeGraph.ViewModel.Nodes.Data
{
    public class ConstantStringNode : LinqSTGNodeViewModel
    {
        public StringValueEditorViewModel ValueEditor { get; } = new StringValueEditorViewModel();
        public LinqSTGNodeOutputViewModel<Contextual<string>> OutputValue { get; }

        public ConstantStringNode()
        {
            OutputValue = LinqSTGNodeOutputViewModel.String(editor: ValueEditor);

            AddOutput("value", OutputValue);
            AddEditor("value", ValueEditor);

            Name = "String";

            TitleColor = NodeColors.Data;
        }
    }
}
