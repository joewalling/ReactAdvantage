import React from 'react';
import Enzyme, { shallow, render } from 'enzyme';

import Header from './index';

it('renders correctly', () => {
    const component = shallow(<Header />);
    expect(component).toMatchSnapshot();
});

describe('header controls are showing after click on controls button', () => {
    const component = shallow(<Header />);

    test('controls is hidden by default', () => {
        expect(component.state().controlsVisible).toBe(false);
    });

    test('controls is visible after click on controls button', () => {
        component.find('.controls-button').simulate('click');
        expect(component.state().controlsVisible).toBe(true);
        expect(component).toMatchSnapshot();
    })
});

describe('mobile menu is working', () => {
    global.innerWidth = 1024;
    const component = shallow(<Header />);

    test('mobile menu is visible after click on trigger button', () => {
        component.find('.header-bar-button').simulate('click');
        expect(component.state().mobileMenuVisible).toBe(true);
    });

    test('mobile menu is hidden after resize', () => {
        global.innerWidth = 1026;
        global.dispatchEvent(new Event('resize'));
        expect(component.update().state().mobileMenuVisible).toBe(false);
    });
});