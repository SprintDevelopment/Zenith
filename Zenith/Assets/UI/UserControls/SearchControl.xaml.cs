using System.Linq;
using System.Reflection;
using System.Windows.Controls;
using System.Windows.Data;
using Zenith.Assets.Attributes;
using Zenith.Assets.UI.Helpers;
using Zenith.Assets.Values.Dtos;

namespace Zenith.Assets.UI.UserControls
{
    /// <summary>
    /// Interaction logic for SearchControl.xaml
    /// </summary>
    public partial class SearchControl : UserControl
    {
        public SearchControl()
        {
            InitializeComponent();
        }
        public void Initialize(SearchBaseDto model)
        {
            TitleTextBlock.Text = model.Title;
            SearchMemberStackPanel.Children.Clear();

            var SearchProperties = model.GetType().GetProperties().Where(p => p.GetCustomAttribute(typeof(SearchAttribute), false) != null).ToList();

            foreach (var property in SearchProperties)
            {
                var searchAttribute = property.GetCustomAttribute(typeof(SearchAttribute)) as SearchAttribute;

                if (searchAttribute != null)
                {
                    //switch (searchAttribute.SearchControlEnum)
                    //{
                    //    case SearchControlType.TextBox:
                    //        {
                    var newTextBox = UiGenerator.GetTextBox(searchAttribute.Title);
                    var binding = new Binding(property.Name);
                    binding.Source = model;
                    binding.UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged;
                    newTextBox.SetBinding(TextBox.TextProperty, binding);
                    SearchMemberStackPanel.Children.Add(newTextBox);
                    //        }
                    //        break;

                    //    case SearchControlType.ComboBox:
                    //        {
                    //            var newComboBox = UiUtil.GetComboBox(searchAttribute.Title);
                    //            newComboBox.Name = $"_{propertyPassNumber}";

                    //            if (!searchAttribute.EnumOrRepositoryName.Contains("Repository"))
                    //            {
                    //                newComboBox.DisplayMemberPath = "DisplayMember";
                    //                newComboBox.SelectedValuePath = "IntegerValueMember";

                    //                var enumType = Type.GetType($"SOORIN.Asset.Enums.{searchAttribute.EnumOrRepositoryName}");
                    //                newComboBox.ItemsSource = EnumUtil.EnumTypeToItemsSourceCollection(enumType, true);
                    //            }
                    //            else
                    //            {
                    //                var modelName = searchAttribute.EnumOrRepositoryName.Split(new string[] { "Repository" }, StringSplitOptions.RemoveEmptyEntries)[0];
                    //                var modelType = Type.GetType($"SOORIN.DataAccess.Model.{modelName}");
                    //                ModelAttribute modelAttribute = modelType.GetCustomAttribute(typeof(ModelAttribute)) as ModelAttribute;

                    //                newComboBox.SelectedValuePath = modelAttribute.KeyPropertyName;
                    //                newComboBox.DisplayMemberPath = modelAttribute.TitlePropertyName;

                    //                var repositoryType = Type.GetType($"SOORIN.DataAccess.EF.{searchAttribute.EnumOrRepositoryName}");
                    //                var getAllMethod = searchAttribute.HasDontCare ? repositoryType.GetMethod("GetAllWithDontCare") : repositoryType.GetMethod("GetAll");

                    //                if (getAllMethod.GetParameters().Count() > 0)
                    //                    newComboBox.ItemsSource = (IList)getAllMethod.Invoke(repositoryType, new object[] { false }); // containsDeleted
                    //                else
                    //                    newComboBox.ItemsSource = (IList)getAllMethod.Invoke(repositoryType, null);
                    //            }

                    //            newComboBox.SelectionChanged += (s, e) =>
                    //            {
                    //                toDisplayList.Clear();
                    //                var thisPropertyPassNumber = int.Parse((s as ComboBox).Name.Substring(1));
                    //                var propertyPassNumberReverse = totalPassNumber - thisPropertyPassNumber;

                    //                for (int i = 0; i < allList.Count(); i++)
                    //                {
                    //                    if (Convert.ToInt64(newComboBox.SelectedValue) == Constants.DONT_CARE_VALUE || Convert.ToInt64(property.GetValue(allList[i])) == Convert.ToInt64(newComboBox.SelectedValue))
                    //                        NotPassed[i] &= propertyPassNumberReverse;
                    //                    else
                    //                        NotPassed[i] |= thisPropertyPassNumber;

                    //                    if (NotPassed[i] == 0 || allList[i].IsTotalRow)
                    //                        toDisplayList.Add(allList[i]);
                    //                }
                    //                toDisplayList.SetModelOrderProperty();
                    //            };
                    //            SearchMemberStackPanel.Children.Add(newComboBox);

                    //            if (searchPrimaryModel != null)
                    //            {
                    //                newComboBox.SelectedValue = property.GetValue(searchPrimaryModel);
                    //                newComboBox.RaiseEvent(
                    //                    new SelectionChangedEventArgs(Selector.SelectionChangedEvent,
                    //                    new List<ComboBoxItem> { newComboBox.Items[0] as ComboBoxItem },
                    //                    new List<ComboBoxItem> { newComboBox.SelectedItem as ComboBoxItem }));
                    //            }
                    //        }
                    //        break;

                    //    case SearchControlType.ComboBoxUC:
                    //        {
                    //            var newComboBoxUC = UiUtil.GetComboBoxUC(searchAttribute.Title);
                    //            newComboBoxUC.Name = $"_{propertyPassNumber}";

                    //            var modelName = searchAttribute.EnumOrRepositoryName.Split(new string[] { "Repository" }, StringSplitOptions.RemoveEmptyEntries)[0];
                    //            var modelType = Type.GetType($"SOORIN.DataAccess.Model.{modelName}");

                    //            if (modelType == typeof(Person))
                    //                newComboBoxUC.ItemsSource = GenericUtil.ToObservableCollectionOfModel(PersonRepository.GetAllWithDontCare());
                    //            else if (modelType == typeof(Product))
                    //                newComboBoxUC.ItemsSource = GenericUtil.ToObservableCollectionOfModel(ProductRepository.GetAllWithDontCare());

                    //            newComboBoxUC.SelectionChanged += (s, e) =>
                    //            {
                    //                toDisplayList.Clear();
                    //                var thisPropertyPassNumber = int.Parse((s as ComboBoxUC).Name.Substring(1));
                    //                var propertyPassNumberReverse = totalPassNumber - thisPropertyPassNumber;

                    //                for (int i = 0; i < allList.Count(); i++)
                    //                {
                    //                    if (Convert.ToInt64(newComboBoxUC.SelectedValue) < 0 || Convert.ToInt64(property.GetValue(allList[i])) == Convert.ToInt64(newComboBoxUC.SelectedValue))
                    //                        NotPassed[i] &= propertyPassNumberReverse;
                    //                    else
                    //                        NotPassed[i] |= thisPropertyPassNumber;

                    //                    if (NotPassed[i] == 0 || allList[i].IsTotalRow)
                    //                        toDisplayList.Add(allList[i]);
                    //                }
                    //                toDisplayList.SetModelOrderProperty();
                    //            };
                    //            SearchMemberStackPanel.Children.Add(newComboBoxUC);

                    //            if (searchPrimaryModel != null)
                    //            {
                    //                newComboBoxUC.SelectedValue = property.GetValue(searchPrimaryModel);
                    //            }
                    //        }
                    //        break;

                    //    case SearchControlType.DatePickerUC:
                    //        {
                    //            if (!searchAttribute.HasStartAndEndControlForDates)
                    //            {
                    //                var newDatePickerUC = GetDatePickerUC(searchAttribute);
                    //                newDatePickerUC.Name = $"_{propertyPassNumber}";

                    //                newDatePickerUC.DateChanged += (s, e) =>
                    //                {
                    //                    toDisplayList.Clear();
                    //                    var thisPropertyPassNumber = int.Parse((s as DatePickerUC).Name.Substring(1));
                    //                    var propertyPassNumberReverse = totalPassNumber - thisPropertyPassNumber;

                    //                    for (int i = 0; i < allList.Count(); i++)
                    //                    {
                    //                        var propertyValue = property.GetValue(allList[i]).ToString();
                    //                        if (string.IsNullOrEmpty(newDatePickerUC.Date) || propertyValue.Contains(newDatePickerUC.Date))
                    //                            NotPassed[i] &= propertyPassNumberReverse;
                    //                        else
                    //                            NotPassed[i] |= thisPropertyPassNumber;

                    //                        if (NotPassed[i] == 0 || allList[i].IsTotalRow)
                    //                            toDisplayList.Add(allList[i]);
                    //                    }
                    //                    toDisplayList.SetModelOrderProperty();
                    //                };
                    //                SearchMemberStackPanel.Children.Add(newDatePickerUC);

                    //                if (searchPrimaryModel != null)
                    //                    newDatePickerUC.Date = property.GetValue(searchPrimaryModel).ToString();
                    //            }
                    //            else
                    //            {
                    //                var newDatePickerUCForStart = GetDatePickerUC(searchAttribute, " (شروع)");
                    //                newDatePickerUCForStart.Name = $"S_{propertyPassNumber}";

                    //                var newDatePickerUCForEnd = GetDatePickerUC(searchAttribute, " (پایان)");
                    //                newDatePickerUCForEnd.Name = $"E_{propertyPassNumber}";

                    //                newDatePickerUCForStart.DateChanged += (s, e) =>
                    //                {
                    //                    toDisplayList.Clear();
                    //                    var thisPropertyPassNumber = int.Parse((s as DatePickerUC).Name.Substring(2));
                    //                    var propertyPassNumberReverse = totalPassNumber - thisPropertyPassNumber;

                    //                    var startDate = newDatePickerUCForStart.Date;
                    //                    var endDate = newDatePickerUCForEnd.Date;

                    //                    if (string.IsNullOrWhiteSpace(startDate))
                    //                        startDate = "0000/00/00";

                    //                    if (string.IsNullOrWhiteSpace(endDate))
                    //                        endDate = "9999/99/99";

                    //                    for (int i = 0; i < allList.Count(); i++)
                    //                    {
                    //                        var propertyValue = property.GetValue(allList[i]).ToString();

                    //                        if ((propertyValue.CompareTo(startDate) >= 0 && propertyValue.CompareTo(endDate) <= 0))
                    //                            NotPassed[i] &= propertyPassNumberReverse;
                    //                        else
                    //                            NotPassed[i] |= thisPropertyPassNumber;

                    //                        if (NotPassed[i] == 0 || allList[i].IsTotalRow)
                    //                            toDisplayList.Add(allList[i]);
                    //                    }
                    //                    toDisplayList.SetModelOrderProperty();
                    //                };
                    //                newDatePickerUCForEnd.DateChanged += (s, e) =>
                    //                {
                    //                    toDisplayList.Clear();
                    //                    var thisPropertyPassNumber = int.Parse((s as DatePickerUC).Name.Substring(2));
                    //                    var propertyPassNumberReverse = totalPassNumber - thisPropertyPassNumber;

                    //                    var startDate = newDatePickerUCForStart.Date;
                    //                    var endDate = newDatePickerUCForEnd.Date;

                    //                    if (string.IsNullOrWhiteSpace(startDate))
                    //                        startDate = "0000/00/00";

                    //                    if (string.IsNullOrWhiteSpace(endDate))
                    //                        endDate = "9999/99/99";

                    //                    for (int i = 0; i < allList.Count(); i++)
                    //                    {
                    //                        var propertyValue = property.GetValue(allList[i]).ToString();

                    //                        if ((propertyValue.CompareTo(startDate) >= 0 && propertyValue.CompareTo(endDate) <= 0))
                    //                            NotPassed[i] &= propertyPassNumberReverse;
                    //                        else
                    //                            NotPassed[i] |= thisPropertyPassNumber;

                    //                        if (NotPassed[i] == 0 || allList[i].IsTotalRow)
                    //                            toDisplayList.Add(allList[i]);
                    //                    }
                    //                    toDisplayList.SetModelOrderProperty();
                    //                };

                    //                SearchMemberStackPanel.Children.Add(newDatePickerUCForStart);
                    //                SearchMemberStackPanel.Children.Add(newDatePickerUCForEnd);

                    //                if (searchPrimaryModel != null)
                    //                {
                    //                    newDatePickerUCForStart.Date = property.GetValue(searchPrimaryModel).ToString();
                    //                    newDatePickerUCForEnd.Date = property.GetValue(searchPrimaryModel).ToString();
                    //                }
                    //            }
                    //        }
                    //        break;
                    //    case SearchControlType.CheckBox:
                    //        break;
                    //    default:
                    //        break;
                    //}
                }
            }

            if (SearchMemberStackPanel.Children.Count > 0)
                SearchMemberStackPanel.Children[0].Focus();
        }
    }
}
