import React from 'react';
import Enzyme, { shallow } from 'enzyme';

import Tree from './index';

it('renders correctly', () => {
    const component = shallow(<Tree />);

    expect(component).toMatchSnapshot();
});
