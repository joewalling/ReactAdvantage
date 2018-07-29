import React from 'react';
import Enzyme, { shallow } from 'enzyme';

import Button from './index';

test('default button renders correctly', () => {
    const component = shallow(<Button />);

    expect(component).toMatchSnapshot();
});

test('secondary button renders correctly', () => {
    const component = shallow(<Button secondary />);

    expect(component).toMatchSnapshot();
});

