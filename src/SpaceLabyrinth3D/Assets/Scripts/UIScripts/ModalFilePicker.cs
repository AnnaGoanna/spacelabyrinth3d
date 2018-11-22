using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class ModalFilePicker : MonoBehaviour
{
    public delegate void OnSubmit(string fileName);

    private static readonly string _extension = "level";
    private static readonly string _defaultLevelName = "exampleMap";
    private string _directory;

    protected Toggle _FilePrefab;

    protected RectTransform _DialogPanel;
    protected Text _TitleText;
    protected ScrollRect _FilesScrollView;
    protected ToggleGroup _FileOptionsGroup;
    protected RectTransform _ButtonPanel;
    protected InputField _ChosenFileInputField;
    protected Button _OkButton;

    protected RectTransform _MessagePanel;
    protected Text _MessageText;
    protected Button _MessageOkButton;

    private void Awake()
    {
        /* Set parameters */
        _directory = Application.streamingAssetsPath + "/Levels/Custom";

        /* Load prefabs */
        _FilePrefab = Resources.Load<Toggle>("Prefabs/FileToggleOption");

        /* Prepare file options */
        if (!Directory.Exists(_directory))
        {
            Directory.CreateDirectory(_directory);
        }

        /* Get necessary children */
        RectTransform[] dialogs = gameObject.GetComponentsInChildren<RectTransform>(true);
        foreach (RectTransform dialog in dialogs)
        {
            if (dialog.name.Equals("ChooseFileDialog")) _DialogPanel = dialog;
            if (dialog.name.Equals("MessageDialog")) _MessagePanel = dialog;
        }
        /* file picker children */
        _TitleText = _DialogPanel.GetComponentInChildren<Text>(true);
        _FilesScrollView = _DialogPanel.GetComponentInChildren<ScrollRect>(true);
        _FileOptionsGroup = _FilesScrollView.GetComponentInChildren<RectTransform>(true).GetComponentInChildren<ToggleGroup>(true);
        _ButtonPanel = _DialogPanel.GetComponentInChildren<RectTransform>(true);
        _ChosenFileInputField = _ButtonPanel.GetComponentInChildren<InputField>(true);
        _OkButton = _ButtonPanel.GetComponentInChildren<Button>(true);

        /* message children */
        _MessageText = _MessagePanel.GetComponentInChildren<Text>(true);
        _MessageOkButton = _MessagePanel.GetComponentInChildren<Button>(true);


        /* Setup non-varying object properties */
        _DialogPanel.gameObject.SetActive(false);
        _MessagePanel.gameObject.SetActive(false);
        _FileOptionsGroup.allowSwitchOff = true;
        _ChosenFileInputField.placeholder.GetComponent<Text>().text = _defaultLevelName;
    }

    private void PopulateFileOptions()
    {
        /* Clear options */
        Toggle[] toggles = _FileOptionsGroup.GetComponentsInChildren<Toggle>();
        foreach (Toggle t in toggles)
        {
            _FileOptionsGroup.UnregisterToggle(t);
            Destroy(t.gameObject);
        }

        /* List options */
        string[] fullPaths = Directory.GetFiles(_directory, "*." + _extension);
        foreach (string filePath in fullPaths)
        {
            string name = Path.GetFileNameWithoutExtension(filePath);

            Toggle fileOption = Instantiate(_FilePrefab);
            fileOption.name = name;
            fileOption.GetComponentInChildren<Text>().text = name;
            fileOption.onValueChanged.AddListener(delegate
            {
                FileOptionChanged(fileOption);
            });
            fileOption.group = _FileOptionsGroup;
            fileOption.transform.SetParent(_FileOptionsGroup.transform);
            fileOption.transform.localPosition = new Vector3(fileOption.transform.localPosition.x, fileOption.transform.localPosition.y, 0.0f);
            fileOption.transform.localScale = Vector3.one;
            fileOption.isOn = false;
            fileOption.gameObject.SetActive(true);
        }
    }

    private void FileOptionChanged(Toggle toggle)
    {
        if (toggle.isOn) _ChosenFileInputField.text = toggle.name;
    }


    private string GetFullFileName(string name)
    {
        return _directory + "/" + name + "." + _extension;
    }


    private void ShowDialog()
    {
        PopulateFileOptions();
        _DialogPanel.parent.gameObject.SetActive(true);
        _DialogPanel.gameObject.SetActive(true);
    }

    private void ShowDialog(string title, string okButtonLabel, bool inputfieldEnabled, OnSubmit onOkButtonClick)
    {
        _TitleText.text = title;
        _OkButton.GetComponentInChildren<Text>().text = okButtonLabel;
        _ChosenFileInputField.enabled = inputfieldEnabled;

        _OkButton.onClick.AddListener(delegate
        {
            string name = _ChosenFileInputField.text;
            if (string.IsNullOrEmpty(name)) name = _defaultLevelName;
            onOkButtonClick(GetFullFileName(name));
        });

        ShowDialog();
    }

    private void ShowMessage(string text, string okButtonLabel, OnSubmit onOkButtonClick)
    {
        _MessageText.text = text;
        _MessageOkButton.GetComponentInChildren<Text>().text = okButtonLabel;

        if (onOkButtonClick != null)
        {
            _MessageOkButton.onClick.AddListener(delegate { onOkButtonClick(""); });
        }

        _MessagePanel.parent.gameObject.SetActive(true);
        _MessagePanel.gameObject.SetActive(true);
    }


    public static void InstantiateModalDialog(string dialogName, string title, string okButtonLabel, bool inputfieldEnabled, OnSubmit onOkButtonClick)
    {
        ModalFilePicker modalFilePicker;
        GameObject modalDialogObject = GameObject.Find("Canvas").FindObject(dialogName);

        if (modalDialogObject == null)
        {
            RectTransform modalDialogPrefab = Resources.Load<RectTransform>("Prefabs/ModalFilePicker");
            modalDialogObject = Instantiate(modalDialogPrefab, modalDialogPrefab.transform.position, modalDialogPrefab.transform.rotation).gameObject;
            modalDialogObject.name = dialogName;
            modalDialogObject.transform.SetParent(GameObject.Find("Canvas").transform, false);

            modalFilePicker = modalDialogObject.GetComponent<ModalFilePicker>();
            if (modalFilePicker == null) modalFilePicker = modalDialogObject.AddComponent<ModalFilePicker>();
            modalFilePicker.ShowDialog(title, okButtonLabel, inputfieldEnabled, onOkButtonClick);
        }
        else
        {
            modalFilePicker = modalDialogObject.GetComponent<ModalFilePicker>();
            modalFilePicker.ShowDialog();
        }
    }

    public static void InstantiateMessageDialog(string text, string okButtonLabel, OnSubmit onOkButtonClick)
    {
        string dialogName = "MessagePopup";
        GameObject modalDialogObject = GameObject.Find("Canvas").FindObject(dialogName);

        if (modalDialogObject == null)
        {
            RectTransform modalDialogPrefab = Resources.Load<RectTransform>("Prefabs/ModalFilePicker");
            modalDialogObject = Instantiate(modalDialogPrefab, modalDialogPrefab.transform.position, modalDialogPrefab.transform.rotation).gameObject;
            modalDialogObject.name = dialogName;
            modalDialogObject.transform.SetParent(GameObject.Find("Canvas").transform, false);
        }

        ModalFilePicker modalFilePicker = modalDialogObject.GetComponent<ModalFilePicker>();
        if (modalFilePicker == null) modalFilePicker = modalDialogObject.AddComponent<ModalFilePicker>();
        modalFilePicker.ShowMessage(text, okButtonLabel, onOkButtonClick);
    }
}
