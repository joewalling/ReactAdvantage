import React from 'react';
import Enzyme, { shallow } from 'enzyme';

import PageHeader from './index';

it('renders correctly', () => {
    const component = shallow(<PageHeader />);

    expect(component).toMatchSnapshot();
});
