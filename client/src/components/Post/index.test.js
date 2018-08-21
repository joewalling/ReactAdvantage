import React from 'react';
import Enzyme, { shallow } from 'enzyme';

import Post from './index';

it('renders correctly', () => {
    const component = shallow(<Post />);

    expect(component).toMatchSnapshot();
});