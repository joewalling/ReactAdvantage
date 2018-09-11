import React, { Component } from 'react';
import { Link } from 'react-router-dom';

import Tag from 'components/Tag';
import './index.css';

export default class Article extends Component {
    render() {
        const {
            article: {
                date = '10th may, 2018',
                title = 'New and improved teamwork desc and teamwork projects integration',
                text = 'Sed ut perspiciatis, unde omnis iste natus error sit voluptatem accusantium doloremque laudantium, totam rem aperiam eaque ipsa, quae ab illo inventore veritatis et quasi architecto beatae vitae dicta sunt, explicabo. Nemo enim ipsam voluptatem, quia voluptas sit, aspernatur aut odit aut fugit, sed quia consequuntur magni dolores eos, qui ratione voluptatem sequi nesciunt, neque porro quisquam est, qui dolorem ipsum, quia dolor sit, amet, consectetur, adipisci velit, sed quia non numquam eius modi tempora incidunt, ut labore et dolore magnam aliquam quaerat voluptatem.',
                url = '/',
                type = this.props.index % 2,
            } = {}
        } = this.props;

        const tagText = type
            ? 'Enhancement'
            : 'Announcement';

        return (
            <article className="article">
                <div className="article-top">
                    <Tag
                        text={tagText}
                        type={tagText.toLowerCase()}
                    />
                    <div className="article-date">{`${date}`}</div>
                </div>

                <div className="article-bottom">
                    <h4 className="article-title">{title}</h4>
                    <p className="article-text">{text}</p>
                    <Link className="article-link" to={url}>
                        Learn more
                    </Link>
                </div>
            </article>
        );
    }
}
