using EncryptMessanger.dll.Encription;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EncryptMessangerClient
{
    class EncryptedSessionManager
    {
        private string _fileName;
        private long _clientId;
        private SessionIO _io;
        private List<ClientClientEncryptedSession> _sessions = new List<ClientClientEncryptedSession>();
        public EncryptedSessionManager(string fileName, long clientId)
        {
            _fileName = fileName;
            _clientId = clientId;
            _io = new SessionIO();
        }
        public ClientClientEncryptedSession LoadSession(long dialogId)
        {
            ClientClientEncryptedSession session = _io.LoadSession(dialogId, _clientId, _fileName);
            _sessions.Add(session);           
            return session;
        }
        
        public ClientClientEncryptedSession FindSession(long dialogId)
        {
            for (int i = 0; i < _sessions.Count; i++)
            {
                if (_sessions[i].Dialog == dialogId)
                {
                    return _sessions[i];
                }
            }
            try
            {
                ClientClientEncryptedSession session = LoadSession(dialogId);
                _sessions.Add(session);
                return session;
            }
            catch (Exception)
            {
                return null;
            }
        }
        public void SessionAddOrReplase(ClientClientEncryptedSession session)
        {
            //SessionIO io = new SessionIO();
            for (int i = 0; i < _sessions.Count; i++)
            {
                if (_sessions[i].Dialog == session.Dialog)
                {
                    _sessions[i] = session;
                    _io.Save(_fileName, _clientId, session);
                    return;
                }
            }
            _sessions.Add(session);
            _io.Save(_fileName, _clientId, session);
        }
    }
}
