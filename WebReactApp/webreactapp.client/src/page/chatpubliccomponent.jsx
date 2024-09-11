import PropTypes from 'prop-types';
import { forwardRef, useEffect, useState } from 'react';
import useWebSocket, { ReadyState } from 'react-use-websocket';

const ChatPublicComponent = forwardRef(function ChatPublicComponent(props, ref) {

    var loc = window.location, new_uri;
    if (loc.protocol === "https:") {
        new_uri = "wss:";
    } else {
        new_uri = "ws:";
    }
    new_uri += "//" + loc.host;
    new_uri += loc.pathname + "/api/listen/publicchat/publicchannel";

    const [currentMessages, setCurrentMessages] = useState('');
    const [currentMessageText, setCurrentMessageText] = useState('');
    const { sendwsMessage, lastwsMessage, wsreadyState } = useWebSocket(new_uri);

    useEffect(() => {
        if (lastwsMessage !== null) {
            setCurrentMessages((prev) => prev.concat(lastwsMessage));
        }
    }, [lastwsMessage]);

    useEffect(() => {
    }, []);

    const currentMessageChangeHandler = (e) => {
        setCurrentMessageText(e.target.value);
    }
    const sendHandler = () => {
    }

    return (
        <div>
            <div className="row">
                <div className="col">
                    <h1>Public Chat</h1>
                </div>
            </div>
            <div className="row">
                <div className="col">
                    <textarea rows="10" className="form-control" id="chattextarea" value={currentMessages}/>
                </div>
            </div>
            <div className="row">
                <div className="col">
                    <label htmlFor="messageform_messagetext">
                        Message send:
                    </label>
                    <input type="text" className="form-control" id="messageform_messagetext" value={currentMessageText} onChange={currentMessageChangeHandler} />
                    <button type="button" className="btn btn-primary" onClick={sendHandler}>Send</button>
                </div>
            </div>
        </div>
    );
});
ChatPublicComponent.propTypes = {
    loginstatuschecker: PropTypes.func,
    loginnotifier: PropTypes.func
};
export default ChatPublicComponent;