# NotifyIconSample
Minimalist example of NotifyIcon app with initially invisible form.  Adapted from .NET [NotifyIcon Class](https://docs.microsoft.com/en-us/dotnet/api/system.windows.forms.notifyicon?view=netframework-4.7.2) Example
## Background
Years ago, I wrote ANSI C Win32 utilities using [Rector & Newcomer "Win32 Programming"](https://www.amazon.com/dp/B006PU8B8C).  
So much as possible since then, C++ and objective C  were avoided *because ugly*.  
C# seems not unpleasant in appearance, but some syntax is unobvious  
and some classes do __NOT__ behave intuitively.

The eventual goal is filter programs that mostly sit quietly on the notification bar,  
but pop up balloons, dialog boxes and/or frames as appropriate.  
After trying to hack other GitHub repositories to suit,  
first sorting the proposed user interface seems a better approach.  
Meanwhile, Albahari's ["C# 7.0 Pocket Reference"](https://www.amazon.com/gp/product/1491988533) is on the way..  
* Initial Form State is Normal, Visible == False, ShowInTaskbar == True 
* 8 Form Resize events before first user-initiated minimize
* Application form defaults to hidden in the constructor,
  but becomes immediately visible after unless minimized and hidden on the taskbar.
* Calling class main methods from other classes requires a Application.Run() with a public static instance.
* Calling other class methods from main wants a readonly instance of that class.
