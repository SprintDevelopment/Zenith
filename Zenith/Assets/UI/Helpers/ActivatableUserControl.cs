using ReactiveUI;
using System;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using System.Windows.Controls;

namespace Zenith.Assets.UI.Helpers
{
    public class ActivatableUserControl : UserControl, IActivationForViewFetcher, ICanActivate
    {
        #region Implements IActivationForViewFetcher
        public int GetAffinityForView(Type view) => 1;
        public IObservable<bool> GetActivationForView(IActivatableView view) => Observable.FromEventPattern(this, nameof(Loaded)).Select(x => true);
        #endregion

        #region Implements ICanActivate
        public IObservable<Unit> Activated => Observable.FromEventPattern(this, nameof(Loaded)).Select(x => Unit.Default);
        public IObservable<Unit> Deactivated => Observable.FromEventPattern(this, nameof(Unloaded)).Select(x => Unit.Default);
        #endregion
    }
}
