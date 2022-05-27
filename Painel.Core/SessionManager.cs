using Painel.Models;
using System;
using System.Web;

namespace Painel.Core
{
    [Serializable()]
    public class SessionManager
    {
        #region Engine

        private SessionManager()
        {
        }

        private static bool _IsNewSession;

        private static string _SessionName;

        public bool IsNewSession
        {
            get { return _IsNewSession; }
        }

        public string SessionName
        {
            get { return _SessionName; }
        }

        public static SessionManager GetInstance()
        {
            var sessionControler = new SessionManager();

            _SessionName = sessionControler.GetType().FullName;

            if (HttpContext.Current.Session[_SessionName] == null)
            {
                _IsNewSession = true;
                HttpContext.Current.Session.Add(_SessionName, sessionControler);                
            }
            else
            {
                _IsNewSession = false;
            }

            HttpContext.Current.Session.Timeout = 40;

            return (SessionManager)HttpContext.Current.Session[_SessionName];
        }

        public static SessionManager GetInstance(HttpContext httpcontext)
        {
            var sessionControler = new SessionManager();

            _SessionName = sessionControler.GetType().FullName;

            if (httpcontext.Session[_SessionName] == null)
            {
                _IsNewSession = true;
                httpcontext.Session.Add(_SessionName, sessionControler);
            }
            else
            {
                _IsNewSession = false;
            }

            httpcontext.Session.Timeout = 40;

            return (SessionManager)httpcontext.Session[_SessionName];
        }

        public void Remove()
        {
            HttpContext.Current.Session.Remove(this.GetType().FullName);
        }

        #endregion              

        private Usuario _usuario;
        public Usuario Usuario
        {
            get
            {
                if (_usuario == null) { _usuario = new Usuario(); }
                return _usuario;
            }
            set { _usuario = value; }
        }
    }
}