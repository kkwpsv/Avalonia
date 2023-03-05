using System;
using System.Collections.Generic;
using OpenQA.Selenium;
using OpenQA.Selenium.Appium;

namespace Avalonia.IntegrationTests.Appium.Crapium;

public class UIElement : IElement
{
    private readonly IWebElement _inner;
    
    public UIElement(IWebElement inner)
    {
        _inner = inner;
    }

    protected IWebElement Inner => _inner;
    
    public string Name => _inner.GetAttribute("title");

    public string Value => _inner.Text;

    public virtual void Click()
    {
        _inner.Click();
    }

    public void SendKeys(string text)
    {
        _inner.SendKeys(text);
    }
    
    public IElement FindElement(string id)
    {
        var f = MacSession.GetElementFactory();
        return (IElement)f(_inner.FindElement(MobileBy.AccessibilityId(id)));
    }
    
    public IElement FindElementByName(string id)
    {
        var f = MacSession.GetElementFactory();
        return (IElement)f(_inner.FindElement(MobileBy.Name(id)));
    }
}