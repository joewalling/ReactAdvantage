import React from 'react';
import Enzyme, { shallow } from 'enzyme';

import Column from './index';

it('renders correctly', () => {
    const component = shallow(<Column />);

    expect(component).toMatchSnapshot();
});