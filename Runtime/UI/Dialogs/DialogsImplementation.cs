// Copyright (c) 2022 Andrei Maksimovich
// See LICENSE.md

#region

using UnityEngine;

#endregion

namespace Amax.UI.Dialogs
{
    public class DialogsImplementation: MonoBehaviour, IDialogs
    {
        
        [field: SerializeField] public RectTransform DialogRoot { get; set; }
        
        private void Awake()
        {
            DialogButtonFactory ??= GetComponent<IDialogButtonFactory>();
            DialogPrefabProvider ??= GetComponent<IDialogPrefabProvider>();
        }
        
        public IMessageDialog ShowMessageDialog(IDialogContent dialogContent)
        {
            (var dialogGO, var dialogView) = InstantiateDialog(dialogContent);
            return new MessageDialog(dialogView);
        }

        public IResultDialog ShowResultDialog(IDialogContent dialogContent)
        {
            (var dialogGO, var dialogView) = InstantiateDialog(dialogContent);
            return new ResultDialog(dialogView);
        }

        private (GameObject dialog, IDialogView) InstantiateDialog(IDialogContent content)
        {
            var prefab = DialogPrefabProvider.GetPrefab(content);
            var dialogGO = Instantiate(prefab, DialogRoot);
            var dialogView = dialogGO.GetComponent<IDialogView>();
            dialogView.Configure(content);
            return (dialogGO, dialogView);
        }

        public IDialogButtonFactory DialogButtonFactory { get; set; }
        public IDialogPrefabProvider DialogPrefabProvider { get; set; }
        
    }
}