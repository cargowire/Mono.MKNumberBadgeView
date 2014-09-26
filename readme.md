# Mono.MKNumberBadgeView

A MonoTouch/Xamarin Forms implementation of [MKNumberBadgeView](https://github.com/michaelkamprath/iPhoneMK/blob/master/Views/MKNumberBadgeView/MKNumberBadgeView.m)
allowing you to render badges on buttons other than TabBarButtons.

# Usage

The BadgeView (UIView) can be used directly as a UIView in a MonoTouch project.  To use the Xamarin Forms implementation
the Badge (View) class can be used in Xaml or added programatically with the BadgeRenderer exported to delegate rendering into the BadgeView.

# Design

By default the BadgeView is drawn with the white border and gradient if the application is running an iOS version less than 7.  Otherwise
a pure red, borderless version will be rendered.
