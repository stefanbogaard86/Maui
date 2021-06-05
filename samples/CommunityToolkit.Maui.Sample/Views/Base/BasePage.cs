using CommunityToolkit.Maui.Sample.Models;
using Microsoft.Maui;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Graphics;
using System;
using System.Windows.Input;

namespace CommunityToolkit.Maui.Sample.Views.Base
{
    public class BasePage : ContentPage, IPage
    {
        ICommand navigateCommand;

        public Color DetailColor { get; set; }

        public ICommand NavigateCommand => navigateCommand ??= new Command<SectionModel>(sectionModel
            => Navigation.PushAsync(PreparePage(sectionModel)));

        Page PreparePage(SectionModel model)
        {
            var page = (BasePage)Activator.CreateInstance(model.Type);
            page.Title = model.Title;
            page.DetailColor = model.Color;
            return page;
        }
    }
}