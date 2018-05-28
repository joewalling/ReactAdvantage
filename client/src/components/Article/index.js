import React, { Component } from 'react';
import { Link } from 'react-router-dom';
import FontAwesomeIcon from '@fortawesome/react-fontawesome';
import faBolt from '@fortawesome/fontawesome-free-solid/faBolt';
import faBullhorn from '@fortawesome/fontawesome-free-solid/faBullhorn';
import './index.css';

export default class Article extends Component {
    render() {
        const {
            date = '10th may, 2018',
            title = 'New and improved teamwork desc and teamwork projects integration',
            text = 'Sed ut perspiciatis, unde omnis iste natus error sit voluptatem accusantium doloremque laudantium, totam rem aperiam eaque ipsa, quae ab illo inventore veritatis et quasi architecto beatae vitae dicta sunt, explicabo. Nemo enim ipsam voluptatem, quia voluptas sit, aspernatur aut odit aut fugit, sed quia consequuntur magni dolores eos, qui ratione voluptatem sequi nesciunt, neque porro quisquam est, qui dolorem ipsum, quia dolor sit, amet, consectetur, adipisci velit, sed quia non numquam eius modi tempora incidunt, ut labore et dolore magnam aliquam quaerat voluptatem.',
            url = '/',
        } = this.props.article;

        const isEnhancement = this.props.index % 2;
        const icon = isEnhancement
            ? faBullhorn
            : faBolt;

        const tagText = isEnhancement
            ? 'Enhancement'
            : 'Announcement';

        return (
            <article className="article">
                <div className="article-top">
                    <div className={`tag${isEnhancement ? ' enhancement' : ''}`}>
                        <div className="tag-icon">
                            <FontAwesomeIcon className="fas" icon={icon} />
                        </div>
                        <div className="tag-text">
                            {tagText}
                        </div>
                    </div>
                    <div className="article-tag">{`${date}`}</div>
                </div>

                <div className="article-bottom">
                    <h4 className="article-title">{title}</h4>
                    <p className="article-text">{text}</p>
                    <Link className="article-link" to={url}>
                        <div>Learn more</div>
                    </Link>
                </div>
            </article>
        );
    }
}
