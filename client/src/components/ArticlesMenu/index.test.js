import React from 'react';
import Enzyme, { shallow } from 'enzyme';

import ArticleMenu from './index';

it('renders correctly', () => {
    const component = shallow(<ArticleMenu />);

    expect(component).toMatchSnapshot();
});

describe('popup is showing after click on button', () => {
    const component = shallow(<ArticleMenu />);
    const button = component.find('.artciles-menu-button');

    test('popup is hidden by default', () => {
        expect(component.state().visible).toEqual(false);
    });

    test('popup is visible after click on trigger button', () => {
        button.simulate('click');
        expect(component.state().visible).toEqual(true);
    });
});
