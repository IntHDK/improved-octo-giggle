import PropTypes from 'prop-types';
import { forwardRef } from 'react';
import { Link } from 'react-router-dom';
import LoginComponent from "./logincomponent";

const HeaderComponent = forwardRef(function HeaderComponent(props, ref) {
    return (
        <div className="row">
            <div className="col-3">
                <h2>
                    <Link to="/">Home</Link>&nbsp;
                </h2>
            </div>
            <div className="col-9">
                <LoginComponent ref={ref} logoutnotifier={props.logoutnotifier} />
            </div>
        </div>
    );
});
HeaderComponent.propTypes = {
    loginnotifier: PropTypes.func,
    logoutnotifier: PropTypes.func
}

export default HeaderComponent;