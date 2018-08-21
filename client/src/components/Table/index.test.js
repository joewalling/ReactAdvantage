import React from 'react';
import Enzyme, { shallow } from 'enzyme';

import Table from './index';

it('renders correctly', () => {
    const component = shallow(<Table />);

    expect(component).toMatchSnapshot();
});