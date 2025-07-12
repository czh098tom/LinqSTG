using DynamicData;
using NodeNetwork.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LinqSTG.Demo.NodeNetwork.ViewModel.Nodes
{
    public class LinqSTGNodeViewModel : NodeViewModel
    {
        private readonly Dictionary<string, NodeEndpointEditorViewModel> editorDict = [];
        private readonly Dictionary<string, NodeInputViewModel> inputDict = [];
        private readonly Dictionary<string, NodeOutputViewModel> outputDict = [];

        public IReadOnlyDictionary<string, NodeEndpointEditorViewModel> EditorDict => editorDict;
        public IReadOnlyDictionary<string, NodeInputViewModel> InputDict => inputDict;
        public IReadOnlyDictionary<string, NodeOutputViewModel> OutputDict => outputDict;

        public void AddOutput(string name, NodeOutputViewModel output)
        {
            if (output is null)
            {
                throw new ArgumentNullException(nameof(output), "Output cannot be null.");
            }
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentException("Output name cannot be null or whitespace.", nameof(name));
            }
            outputDict.Add(name, output);
            Outputs.Add(output);
        }

        public void AddInput(string name, NodeInputViewModel input)
        {
            if (input is null)
            {
                throw new ArgumentNullException(nameof(input), "Input cannot be null.");
            }
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentException("Input name cannot be null or whitespace.", nameof(name));
            }
            inputDict.Add(name, input);
            Inputs.Add(input);
        }

        public void AddEditor(string name, NodeEndpointEditorViewModel editor)
        {
            if (editor is null)
            {
                throw new ArgumentNullException(nameof(editor), "Editor cannot be null.");
            }
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentException("Editor name cannot be null or whitespace.", nameof(name));
            }
            editorDict.Add(name, editor);
        }
    }
}
