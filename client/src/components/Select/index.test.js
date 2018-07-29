import React from 'react';
import Enzyme, { shallow } from 'enzyme';

import Select from './index';

it('renders correctly', () => {
    const component = shallow(<Select />);

    expect(component).toMatchSnapshot();
});