import React, { Component } from 'react';
import ReactDOM from 'react-dom';
import FontAwesomeIcon from '@fortawesome/react-fontawesome';
import faStream from '@fortawesome/fontawesome-free-solid/faStream';
import Sidebar from 'components/Sidebar';
import Article from 'components/Article';
import './index.css';

const modalRoot = document.getElementById('modal-root');

export default class SidebarMenu extends Component {
    componentDidMount() {
        modalRoot.appendChild(this.el);
    }

    componentWillUnmount() {
        modalRoot.removeChild(this.el);
    }

    state = {
        visible: false
    }

    articles = [{}, {}, {}, {}];

    el = document.createElement('div');

    toggleSidebar = () => {
        this.setState({
            visible: !this.state.visisble
        });
    }

    render() {
        const items = this.articles
            .map((article, index) => (<Article key={index} index={index} article={article}/>));

        return (
            <div className="sidebar-menu-wrapper">
                <button onClick={this.toggleSidebar} className="button">
                    <FontAwesomeIcon className="far" icon={faStream} />
                </button>
                {ReactDOM.createPortal(
                  <Sidebar
                        position="right"
                        visible={this.state.visible}
                        onHide={() => this.setState({ visible: false })}
                    >
                        <div className="articles-wrapper">
                            <h2 className="sidebar-title">What's new?</h2>
                            {items}
                        </div>
                    </Sidebar>,
                  this.el,
                )}

            </div>
        );
    }
}
