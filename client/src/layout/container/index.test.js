import React from 'react';
import Enzyme, { shallow, render } from 'enzyme';

import Container from './index';

it('renders correctly', () => {
    const component = shallow(<Container />);
    expect(component).toMatchSnapshot();
});
