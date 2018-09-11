import React from 'react';
import Enzyme, { shallow } from 'enzyme';

import Calendar from './index';

it('renders correctly', () => {
    const component = shallow(<Calendar />);

    expect(component).toMatchSnapshot();
});
