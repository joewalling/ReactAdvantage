import React from 'react';
import Enzyme, { shallow } from 'enzyme';

import ProfileMenu from './index';

it('renders correctly', () => {
    const component = shallow(<ProfileMenu />);

    expect(component).toMatchSnapshot();
});

describe('popup with profile links is showing after click on trigger button', () => {
    const component = shallow(<ProfileMenu />);
    const button = component.find('.photo-button');

    test('profile links are hidden by default', () => {
        expect(component.state().showMenu).toBe(false);
    });

    test('profile links are visible after click on trigger button', () => {
        button.simulate('click');
        expect(component.state().showMenu).toBe(true);
    });

    test('component renders correctly with popup', () => {
        expect(component).toMatchSnapshot();
    });
});

describe('user settings is showing after click on trigger button', () => {
    const component = shallow(<ProfileMenu />);

    test('user settings are hidden by default', () => {
        expect(component.state().showSettings).toBe(false);
    });

    test('user settings are visible after click on trigger button', () => {
        component.setState({ showMenu: true });
        const settingsButton = component.find('.profile-link-settings');
        settingsButton.simulate('click', { preventDefault() {} });
        expect(component.state().showSettings).toBe(true);
    });

    test('component renders correctly with user settings', () => {
        expect(component).toMatchSnapshot();
    });
});
